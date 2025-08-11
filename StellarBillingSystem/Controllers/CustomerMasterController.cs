using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StellarBillingSystem.Context;
using StellarBillingSystem.Controllers;
using System.Diagnostics;
using System.Text.Json;

namespace StellarBillingSystem_skj.Controllers
{
    [Authorize]
    public class CustomerMasterController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _config;

        private readonly BillingContext _billingsoftware;

        public CustomerMasterController(ILogger<HomeController> logger, IConfiguration config, IWebHostEnvironment env, BillingContext billingContext)
        {
            _logger = logger;
            _config = config;
            _env = env;
            _billingsoftware = billingContext;
        }



        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerMasterModel model, List<IFormFile>? Images, string? KeptImages)
        {
            try
            {

                if (TempData["BranchID"] != null)
                {
                    model.BranchID = TempData["BranchID"].ToString();
                    TempData.Keep("BranchID");
                }



                var existingCustomer = await _billingsoftware.SHCustomerMaster.FirstOrDefaultAsync(x => (x.MobileNumber == model.MobileNumber || x.CustomerName == model.CustomerName) &&
          x.BranchID == model.BranchID &&
          x.IsDelete == false);

                if (existingCustomer != null)
                {
                    if (existingCustomer.IsDelete)
                    {
                        ViewBag.Message = "Cannot update. Customer Number is marked as deleted.";

                        return View("CustomerMaster", model);
                    }

                    existingCustomer.CustomerName = model.CustomerName;
                    existingCustomer.DateofBirth = model.DateofBirth;
                    existingCustomer.Gender = model.Gender;
                    existingCustomer.Address = model.Address;
                    existingCustomer.City = model.City;
                    existingCustomer.MobileNumber = model.MobileNumber;
                    existingCustomer.IsDelete = model.IsDelete;
                    existingCustomer.BranchID = model.BranchID;
                    existingCustomer.State = model.State;
                    existingCustomer.Country = model.Country;
                    existingCustomer.Fathername = model.Fathername;
                    existingCustomer.LastUpdatedDate = DateTime.Now.ToString();
                    existingCustomer.LastUpdatedUser = User.Claims.First().Value.ToString();
                    existingCustomer.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    //_billingsoftware.Entry(existingCustomer).State = EntityState.Modified;

                }
                else
                {
                    model.LastUpdatedDate = DateTime.Now.ToString();
                    model.LastUpdatedUser = User.Claims.First().Value.ToString();
                    model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    _billingsoftware.SHCustomerMaster.Add(model);


                }

                await _billingsoftware.SaveChangesAsync();

                ViewBag.Message = "Saved Successfully";

                await SaveCustomerImages(model.MobileNumber, model.CustomerName, Images, KeptImages, model.BranchID);

                CustomerMasterModel mod = new CustomerMasterModel();

                return View("CustomerMaster", mod);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SaveCustomerImages error: {ex}");
                throw;
            }
        }

        public async Task<IActionResult> GetCustomer(CustomerMasterModel model)
        {
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }


            var customer = await _billingsoftware.SHCustomerMaster.FirstOrDefaultAsync(x => x.IsDelete == false && x.MobileNumber == model.MobileNumber && x.BranchID == model.BranchID);

            if (customer == null)
            {
                model = new CustomerMasterModel();
                ViewBag.ErrorMessage = "Customer Number not found";
                return View("CustomerMaster", model); // Return an empty model if not found or deleted
            }

            // 2️⃣ fetch all image paths for that customer
            var imagePaths = await _billingsoftware.ShcustomerImageMaster // DbSet<CustomerImageModel>
                .Where(i => i.MobileNumber == customer.MobileNumber
                         && i.CustomerName == customer.CustomerName
                         && i.BranchID == customer.BranchID
                         && !i.IsDelete)
                .Select(i => i.ImagePath)
                .ToListAsync();

            // 3️⃣ pass the list to the view
            ViewBag.ExistingImages = imagePaths;

            model.Gender = string.IsNullOrEmpty(customer.Gender) ? string.Empty : customer.Gender;

            return View("CustomerMaster", customer);
        }





        public async Task<IActionResult> DeleteCustomer(CustomerMasterModel model)
        {
            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            var existingCustomer = await _billingsoftware.SHCustomerMaster.FirstOrDefaultAsync(x => x.MobileNumber == model.MobileNumber && x.BranchID == model.BranchID);
            if (existingCustomer == null)
            {
                model = new CustomerMasterModel();
                ViewBag.ErrorMessage = "Mobile Number not found";
                return View("CustomerMaster", model);
            }

            if (existingCustomer.IsDelete)
            {
                ViewBag.ErrorMessage = "Customer Number Already Deleted";
                return View("CustomerMaster", model);
            }

            existingCustomer.IsDelete = true;
            existingCustomer.LastUpdatedDate = DateTime.Now.ToString();
            existingCustomer.LastUpdatedUser = User.Claims.First().Value.ToString();
            existingCustomer.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            //  mark related images as deleted
            var checkimage = _billingsoftware.ShcustomerImageMaster
                    .Where(x => x.MobileNumber == model.MobileNumber)
                    .ToList();

            foreach (var image in checkimage)
            {
                var serverPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(serverPath))
                {
                    System.IO.File.Delete(serverPath);
                }
            }

            _billingsoftware.ShcustomerImageMaster.RemoveRange(checkimage);

            var customerImageRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CustomerImages");
            var matchingFolder = Directory.GetDirectories(customerImageRoot)
                .FirstOrDefault(dir => Path.GetFileName(dir).StartsWith(model.MobileNumber));

            if (!string.IsNullOrEmpty(matchingFolder) && Directory.Exists(matchingFolder))
            {
                Directory.Delete(matchingFolder, recursive: true);
            }

            // ✅ Save all at once to avoid context confusion and SQL syntax issues
            _billingsoftware.SaveChanges();

            ViewBag.Message = "Deleted Successfully";
            model = new CustomerMasterModel();

            return View("CustomerMaster", model);
        }


        private async Task SaveCustomerImages(
     string number,                       // MobileNumber
     string name,                         // CustomerName
     List<IFormFile>? images,
     string? keptImagesJson,
     string branchId)                     // BranchID
        {
            try
            {
                // 1️⃣  Build folder path:  wwwroot/CustomerImages/9876543210_Joe_B01
                var safeFolderName = $"{number}_{name}_{branchId}"
                                     .Replace(" ", "_");            // simple sanitise
                var folder = Path.Combine(_env.WebRootPath, "CustomerImages", safeFolderName);
                Directory.CreateDirectory(folder);

                // 2️⃣  Parse kept‑images JSON
                var keptImages = string.IsNullOrWhiteSpace(keptImagesJson)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(keptImagesJson);

                // 3️⃣  Open DB connection
                var connStr = _config.GetConnectionString("BillingDBConnection");
                await using var conn = new SqlConnection(connStr);
                await conn.OpenAsync();

                // 4️⃣  Fetch existing paths for this customer
                var existingImages = new List<string>();
                const string selectSql = @"
            SELECT ImagePath
            FROM shcustomerImageMaster
            WHERE MobileNumber = @MobileNumber
              AND CustomerName = @CustomerName
              AND BranchID     = @BranchID";

                await using (var sel = new SqlCommand(selectSql, conn))
                {
                    sel.Parameters.AddWithValue("@MobileNumber", number);
                    sel.Parameters.AddWithValue("@CustomerName", name);
                    sel.Parameters.AddWithValue("@BranchID", branchId);

                    await using var rdr = await sel.ExecuteReaderAsync();
                    while (await rdr.ReadAsync())
                        existingImages.Add(rdr.GetString(0));
                }

                // 5️⃣  Delete files/rows the user removed
                foreach (var img in existingImages.Where(p => !keptImages.Contains(p)))
                {
                    var phys = Path.Combine(_env.WebRootPath, img.Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(phys)) System.IO.File.Delete(phys);

                    const string delSql = @"
                DELETE FROM shcustomerImageMaster
                WHERE MobileNumber = @MobileNumber
                  AND CustomerName = @CustomerName
                  AND BranchID     = @BranchID
                  AND ImagePath    = @ImagePath";
                    await using var del = new SqlCommand(delSql, conn);
                    del.Parameters.AddWithValue("@MobileNumber", number);
                    del.Parameters.AddWithValue("@CustomerName", name);
                    del.Parameters.AddWithValue("@BranchID", branchId);
                    del.Parameters.AddWithValue("@ImagePath", img);
                    await del.ExecuteNonQueryAsync();
                }

                // 6️⃣  Add new images
                if (images is null) return;

                foreach (var file in images)
                {
                    if (file.Length == 0) continue;

                    var uniqueName = Path.GetFileName(file.FileName);
                    var fullPath = Path.Combine(folder, uniqueName);
                    await using (var fs = new FileStream(fullPath, FileMode.Create))
                        await file.CopyToAsync(fs);

                    var relPath = Path.Combine("CustomerImages", safeFolderName, uniqueName)
                                  .Replace("\\", "/");

                    const string insSql = @"
                INSERT INTO shcustomerImageMaster
                    (MobileNumber, CustomerName, BranchID,
                     ImagePath,   FileName,      IsDelete, IsPrimary)
                VALUES
                    (@MobileNumber, @CustomerName, @BranchID,
                     @ImagePath,    @FileName,     0,        0)";
                    // <-- ImageID auto‑generates
                    await using var ins = new SqlCommand(insSql, conn);
                    ins.Parameters.AddWithValue("@MobileNumber", number);
                    ins.Parameters.AddWithValue("@CustomerName", name);
                    ins.Parameters.AddWithValue("@BranchID", branchId);
                    ins.Parameters.AddWithValue("@ImagePath", relPath);
                    ins.Parameters.AddWithValue("@FileName", file.FileName);
                    await ins.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SaveCustomerImages error: {ex}");
                throw;
            }
        }






        public IActionResult CustomerMaster()
        {
            CustomerMasterModel obj = new CustomerMasterModel();

            return View("CustomerMaster", obj);
        }
    }
}
