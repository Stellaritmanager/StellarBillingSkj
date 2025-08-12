using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StellarBillingSystem.Context;
using StellarBillingSystem_skj.Business;
using System.Data;

namespace StellarBillingSystem_skj.Controllers
{
    [Authorize]
    public class CategoryMasterController : Controller
    {
        private BillingContext _billingsoftware;
        private readonly IConfiguration _configuration;


        public CategoryMasterController(BillingContext billingsoftware, IConfiguration configuration)
        {
            _billingsoftware = billingsoftware;
            _configuration = configuration;
        }

        public async Task<IActionResult> CategoryMaster()
        {
            CategoryMasterModel model = new CategoryMasterModel();
           
            string branchId = HttpContext.Session.GetString("BranchID");
            if (!string.IsNullOrEmpty(branchId))
            {
                model.BranchID = branchId;
            }

            ViewData["Categorydata"] = await AdditionalCategoryMasterFun(model.BranchID);

            return View("CategoryMaster", model);

        }
        public async Task<DataTable> AdditionalCategoryMasterFun(string branchID)
        {

            // Step 1: Perform the query
            var entities = _billingsoftware.SHCategoryMaster
                                  .Where(e => e.BranchID == branchID && e.IsDelete == false).OrderByDescending(e => e.LastUpdatedDate)
                                  .ToList();

            // Step 2: Convert to DataTable
            return BusinessClassCategory.convertToDataTableCategoryMaster(entities);


        }




        [HttpGet]
        public async Task<IActionResult> GetCategory(CategoryMasterModel model, string buttonType)
        {


            string branchId = HttpContext.Session.GetString("BranchID");
            if (!string.IsNullOrEmpty(branchId))
            {
                model.BranchID=branchId;
            }


            if (buttonType == "Get")
            {
                var getcategory = await _billingsoftware.SHCategoryMaster.FirstOrDefaultAsync(x => x.CategoryName == model.CategoryName && !x.IsDelete && x.BranchID == model.BranchID);
                if (getcategory != null)
                {
                    var dataTable = await AdditionalCategoryMasterFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Categorydata"] = dataTable;
                    return View("CategoryMaster", getcategory);
                }
                else
                {
                    CategoryMasterModel par = new CategoryMasterModel();
                    ViewBag.ErrorMessage = "No value for this GoldType";
                    var dataTable = await AdditionalCategoryMasterFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Categorydata"] = dataTable;
                    return View("CategoryMaster", par);
                }
            }

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryMasterModel model, string buttonType)
        {

            BusinessClassCategory business = new BusinessClassCategory(_billingsoftware, _configuration);
            SatffAdminBusinessClass staffbus = new SatffAdminBusinessClass(_billingsoftware, _configuration);


            string branchId = HttpContext.Session.GetString("BranchID");
            if (!string.IsNullOrEmpty(branchId))
            {
                model.BranchID = branchId;
            }


            if (buttonType == "Get")
            {
                var getcategory = await _billingsoftware.SHCategoryMaster.FirstOrDefaultAsync(x => x.CategoryName == model.CategoryName && !x.IsDelete && x.BranchID == model.BranchID);
                if (getcategory != null)
                {
                    var dataTable1 = await AdditionalCategoryMasterFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Categorydata"] = dataTable1;
                    return View("CategoryMaster", getcategory);
                }
                else
                {
                    CategoryMasterModel par = new CategoryMasterModel();
                    ViewBag.ErrorMessage = "No value for this GoldType";
                    var dataTable2 = await AdditionalCategoryMasterFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Categorydata"] = dataTable2;
                    return View("CategoryMaster", par);
                }
            }
            else if (buttonType == "Delete")
            {
                var categorytodelete = await _billingsoftware.SHCategoryMaster.FirstOrDefaultAsync(x => x.CategoryName == model.CategoryName && !x.IsDelete && x.BranchID == model.BranchID);

                if (categorytodelete != null)
                {
                    categorytodelete.IsDelete = true;
                    await _billingsoftware.SaveChangesAsync();

                    ViewBag.Message = "GoldType deleted successfully";

                    var dataTable3 = await AdditionalCategoryMasterFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Categorydata"] = dataTable3;

                    model = new CategoryMasterModel();

                    return View("CategoryMaster", model);
                }
                else
                {
                    ViewBag.ErrorMessage = "GoldType not found";
                    var dataTable4 = await AdditionalCategoryMasterFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Categorydata"] = dataTable4;
                    model = new CategoryMasterModel();
                    return View("CategoryMaster", model);
                }


            }


            else if (buttonType == "save")
            {
                HttpContext.Session.SetString("BranchID", model.BranchID);

                var existingCategory = await _billingsoftware.SHCategoryMaster.FirstOrDefaultAsync(x => x.CategoryName == model.CategoryName && x.BranchID == model.BranchID);

                if (existingCategory != null)
                {
                    if (existingCategory.IsDelete)
                    {
                        ViewBag.ErrorMessage = "Cannot Save or Update. GoldType is marked as deleted.";
                        var dataTable6 = await AdditionalCategoryMasterFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["Categorydata"] = dataTable6;
                        return View("CategoryMaster", model);
                    }
                    //existingCategory.CategoryID = model.CategoryID;
                    existingCategory.CategoryName = model.CategoryName;
                    existingCategory.MarketRate = model.MarketRate;
                    existingCategory.LastUpdatedDate = DateTime.ParseExact(staffbus.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                    existingCategory.LastUpdatedUser = User.Claims.First().Value.ToString();
                    existingCategory.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    _billingsoftware.Entry(existingCategory).State = EntityState.Modified;
                }
                else
                {
                    model.LastUpdatedDate = DateTime.ParseExact(staffbus.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                    model.LastUpdatedUser = User.Claims.First().Value.ToString();
                    model.LastUpdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();


                    _billingsoftware.SHCategoryMaster.Add(model);
                }


                await _billingsoftware.SaveChangesAsync();

                ViewBag.Message = "GoldType Saved Successfully";
            }
            var dataTable = await AdditionalCategoryMasterFun(model.BranchID);

            // Store the DataTable in ViewData for access in the view
            ViewData["Categorydata"] = dataTable;
            model = new CategoryMasterModel();

            return View("CategoryMaster", model);
        }




        public IActionResult Index()
        {
            return View();
        }
    }
}
