using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StellarBillingSystem.Business;
using StellarBillingSystem.Context;
using StellarBillingSystem.Models;
using StellarBillingSystem_skj.Business;
using StellarBillingSystem_skj.Models;
using System.Globalization;
using System.Web;

namespace StellarBillingSystem_skj.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class BillingController : Controller
    {

        private BillingContext _billingsoftware;
        private readonly IConfiguration _configuration;


        public BillingController(BillingContext billingsoftware, IConfiguration configuration)
        {
            _billingsoftware = billingsoftware;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Billing()
        {
            try
            {

                string BranchID = null;

                if (TempData["BranchID"] != null)
                {
                    BranchID = TempData["BranchID"].ToString();
                    TempData.Keep("BranchID");
                }

                BusinessBillingSKJ Busbill = new BusinessBillingSKJ(_billingsoftware, _configuration);
                ViewData["customerid"] = Busbill.getCustomerID(BranchID);
                var goldTypes = Busbill.getGoldtype(BranchID);

                ViewBag.GoldTypeList = goldTypes;
                ViewBag.BranchID = BranchID;


            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
            }
            return View("Billing");
        }

        [HttpPost]
        public async Task<IActionResult> SaveBillWithFiles()
        {
            var form = Request.Form;
            var vmJson = form["vm"];
            BillViewModel vm = JsonConvert.DeserializeObject<BillViewModel>(vmJson);

            if (vm == null)
                return BadRequest("Invalid ViewModel");

            try
            {
                var billMaster = vm.BillMaster;

                var existingBill = _billingsoftware.Shbillmasterskj
                    .FirstOrDefault(b => b.BillID == billMaster.BillID);

                // ✅ Read configured upload path (via DI or IConfiguration)
                string rootPath = _configuration["UploadSettings:BillImagesPath"]; // Example: C:\MyUploads\BillImages

                // ✅ Ensure bill-specific folder exists
                string uploadPath = Path.Combine(rootPath, billMaster.BillID.ToString());
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                // ✅ Save uploaded files and update paths
                foreach (var file in form.Files)
                {
                    string filePath = Path.Combine(uploadPath, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var matchingImage = vm.BillImages.FirstOrDefault(img => img.ImageName == file.FileName);
                    if (matchingImage != null)
                    {
                        // The image is not directly accessible via static path, so store just file name
                        matchingImage.ImagePath = file.FileName;
                    }
                }


                // ✅ Insert or update BillMaster
                if (existingBill != null)
                {
                    _billingsoftware.Entry(existingBill).CurrentValues.SetValues(billMaster);
                }
                else
                {
                    _billingsoftware.Shbillmasterskj.Add(billMaster);
                }

                // ✅ Only insert new articles (do not delete existing ones)
                foreach (var article in vm.Articles)
                {
                    if (article.ArticleID == 0)
                    {
                        _billingsoftware.SHArticleMaster.Add(article);
                    }
                }

                _billingsoftware.SaveChanges(); // Save BillMaster and new Articles

                // ✅ Replace old BillDetails
                var oldDetails = _billingsoftware.Shbilldetailsskj
                    .Where(d => d.BillID == billMaster.BillID).ToList();
                _billingsoftware.Shbilldetailsskj.RemoveRange(oldDetails);

                for (int i = 0; i < vm.BillDetails.Count; i++)
                {
                    var detail = vm.BillDetails[i];
                    var article = vm.Articles[i]; // newly added or existing

                    detail.BillID = billMaster.BillID;
                    detail.ArticleID = article.ArticleID; // use auto-generated ID
                    detail.BranchID = billMaster.BranchID;

                    _billingsoftware.Shbilldetailsskj.Add(detail);
                }

                // ✅ Replace old BillImages
                var oldImages = _billingsoftware.Shbillimagemodelskj
                    .Where(i => i.BillID == billMaster.BillID).ToList();
                _billingsoftware.Shbillimagemodelskj.RemoveRange(oldImages);

                foreach (var image in vm.BillImages)
                {
                    image.BillID = billMaster.BillID;
                    _billingsoftware.Shbillimagemodelskj.Add(image);
                }

                _billingsoftware.SaveChanges();

                return Json(new { success = true, message = "✅ Bill and images saved successfully!" });


            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Exception: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("🔍 Inner: " + ex.InnerException.Message);

                return Json(new { success = false, message = "❌ Error: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetBill(string billId)
        {
            try
            {
                var billMaster = _billingsoftware.Shbillmasterskj
                    .FirstOrDefault(b => b.BillID == billId && b.IsDelete == false);

                if (billMaster == null)
                    return NotFound("Bill not found");

                var billDetails = _billingsoftware.Shbilldetailsskj
                    .Where(d => d.BillID == billId && d.IsDelete == false)
                    .ToList();

                var articleIdList = billDetails
                    .Select(d => d.ArticleID)
                    .Distinct()
                    .ToList();

                // ✅ Safe in-memory filter
                var allArticles = _billingsoftware.SHArticleMaster
                    .AsNoTracking()
                    .ToList();

                var articles = allArticles
                    .Where(a => articleIdList.Contains(a.ArticleID))
                    .ToList();

                var images = _billingsoftware.Shbillimagemodelskj
                    .Where(img => img.BillID == billId)
                    .Select(img => new
                    {
                        img.ImagePath,
                        img.ImageName
                    }).ToList();


                return Json(new
                {
                    BillMaster = billMaster,
                    BillDetails = billDetails,
                    Articles = articles,
                    Images = images
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Exception:", ex.Message);
                return StatusCode(500, "Error retrieving bill");
            }
        }


        [HttpPost]
        public IActionResult DeleteBill([FromForm] string billId, [FromForm] string branchId)
        {
            try
            {
                var getbillmaster = _billingsoftware.Shbillmasterskj
                    .FirstOrDefault(x => x.BillID == billId && !x.IsDelete && x.BranchID == branchId);

                if (getbillmaster == null)
                {
                    return Ok(new { message = "Bill not found or could not be deleted." });
                }

                var checkbillrepleldge = _billingsoftware.Shrepledgeartcile.FirstOrDefault(x => x.BillID == billId && x.BranchID == branchId && x.IsDelete == false);
                if(checkbillrepleldge!=null)
                {
                    return Ok(new { message = "Cannot Delete Bill!! Delete Repledge First" });
                }

                var checkbillpayment = _billingsoftware.SHPaymentMaster.FirstOrDefault(x => x.BillId == billId && x.BranchID == branchId && x.IsDelete == false);
                if (checkbillpayment != null)
                {
                    return Ok(new { message = "Cannot Delete Bill!! Delete Payment First" });
                }

                var getbilldetails = _billingsoftware.Shbilldetailsskj
                    .Where(x => x.BillID == billId && !x.IsDelete && x.BranchID == branchId)
                    .ToList();

                //remove article
                var articleIds = getbilldetails.Select(x => x.ArticleID).Distinct().ToList();

                // 1. Mark master and details as deleted
                getbillmaster.IsDelete = true;

                foreach (var details in getbilldetails)
                {
                    details.IsDelete = true;

                    var articlesToDelete = _billingsoftware.SHArticleMaster
                               .Where(x => x.ArticleID == details.ArticleID)
                               .SingleOrDefault();

                    articlesToDelete.IsDelete = true;
                    


                }

                // 3. Remove images
                var checkimage = _billingsoftware.Shbillimagemodelskj
                    .Where(x => x.BillID == billId)
                    .ToList();

                string rootPath = _configuration["UploadSettings:BillImagesPath"];

                //Delete the Bill Directory. 
                string uploadPath = Path.Combine(rootPath, billId.ToString());
                if (Directory.Exists(uploadPath))
                    Directory.Delete(uploadPath,true);
               
                _billingsoftware.Shbillimagemodelskj.RemoveRange(checkimage);

                var billFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BillImage", billId);
                        if (Directory.Exists(billFolderPath))
                        {
                            Directory.Delete(billFolderPath, recursive: true);
                        }

                // ✅ Save all at once to avoid context confusion and SQL syntax issues
                _billingsoftware.SaveChanges();

                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "An error occurred while deleting the bill." });
            }
        }
        public IActionResult GetBillImage(string billId, string imageName)
        {
            if (string.IsNullOrEmpty(billId) || string.IsNullOrEmpty(imageName))
                return NotFound();

            var root = _configuration["UploadSettings:BillImagesPath"];
         
            var imagePath = Path.Combine(root, billId, imageName);

            if (!System.IO.File.Exists(imagePath))
                return NotFound();

            var contentType = "image/png"; // or use GetContentType(imageName)
            return PhysicalFile(imagePath, contentType);
        }
        [HttpGet]
        public IActionResult GetBillImages(string billId)
        {
            var root = _configuration["UploadSettings:BillImagesPath"];
            var dir = Path.Combine(root, billId);

            if (!Directory.Exists(dir))
                return NotFound();

            var files = Directory.GetFiles(dir)
                                 .Select(Path.GetFileName)
                                 .ToList();

            return Json(files);
        }

        [HttpPost]

        public async Task<IActionResult> printBill([FromForm] string billId, [FromForm] string branchId)

        {
            BusinessBillingSKJ Busbill = new BusinessBillingSKJ(_billingsoftware, _configuration);
            ViewData["customerid"] = Busbill.getCustomerID(branchId);
            var goldTypes = Busbill.getGoldtype(branchId);

            ViewBag.GoldTypeList = goldTypes;
            ViewBag.BranchID = branchId;

            BusinessBillingSKJ busbil = new BusinessBillingSKJ(_billingsoftware,_configuration);

            var checkbillavailable = _billingsoftware.Shbillmasterskj.FirstOrDefault(x => x.BillID == billId && x.BranchID == branchId && x.IsDelete == false);

            if (checkbillavailable == null)
            {
                return BadRequest(new { message = "BillID Not Found" });
            }



            String Query = "SELECT \r\n    SD.BillID,\r\n    CONVERT(VARCHAR(10), SB.BillDate, 101) AS BillDate,\r\n    SD.ArticleID,\r\n    SP.ArticleName,\r\n    FORMAT(TRY_CAST(REPLACE(SB.TotalRepayValue, ',', '') AS DECIMAL(18, 2)), 'N2') AS TotalRepayValue,\r\n    SD.Quantity,\r\n\r\n\r\n    -- Customer Name\r\n    (SELECT CM.CustomerName \r\n     FROM SHCustomerMaster CM \r\n     WHERE CM.CustomerID = SB.CustomerID AND CM.BranchID = SB.BranchID) AS CustomerName,\r\n\r\n    CS.MobileNumber,\r\n    CS.Address,\r\n    CM.CategoryName AS GoldType,\r\n\r\n    FORMAT(TRY_CAST(SD.Grossweight AS DECIMAL(18, 2)), 'N2') AS Grossweight,\r\n    FORMAT(TRY_CAST(SD.Netweight AS DECIMAL(18, 2)), 'N2') AS Netweight,\r\n    FORMAT(TRY_CAST(SD.Reducedweight AS DECIMAL(18, 2)), 'N2') AS Reducedweight,\r\n\r\n    SB.OverallWeight,\r\n    SB.InitialInterest,\r\n    SB.PostTenureInterest,SB.TotalvalueinWords,\r\n\tSB.Tenure\r\nFROM \r\n    Shbilldetailsskj SD\r\nINNER JOIN \r\n    Shbillmasterskj SB ON SD.BillID = SB.BillID\r\nINNER JOIN \r\n    SHArticleMaster SP ON SD.ArticleID = SP.ArticleID\r\nINNER JOIN \r\n    SHCategoryMaster CM ON SP.GoldType = CM.CategoryID\r\nINNER JOIN \r\n    SHCustomerMaster CS ON SB.CustomerID = CS.CustomerID\r\nINNER JOIN \r\n    SHBranchMaster CB ON SB.BranchID = CB.BracnchID\r\n\r\nWHERE \r\n    SD.IsDelete = 0\r\n    AND SD.BillID = '" + billId + "'\r\n    AND SD.BranchID = '" + branchId + "' \r\n    AND SP.BranchID = '" + branchId + "' \r\n    AND CS.BranchID = '" + branchId + "' \r\n    AND CB.BracnchID = '" + branchId + "' \r\n    AND SB.BranchID = '" + branchId + "' \r\n    AND CM.BranchID = '" + branchId + "' \r\n";

            var Table = BusinessClassCommon.DataTable(_billingsoftware, Query);



            // Get current date and time
            var currentDateTime = busbil.GetFormattedDateTime();

            // Create filename with BillID and current datetime
            var fileName = $"{billId}_{busbil.GetFormattedDateTime()}.pdf";
            return File(busbil.PrintBillDetails(Table, branchId), "application/pdf", fileName);



        }


    }
}
