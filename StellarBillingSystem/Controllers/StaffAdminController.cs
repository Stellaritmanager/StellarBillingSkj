using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StellarBillingSystem.Business;
using StellarBillingSystem.Context;
using StellarBillingSystem.Models;
using StellarBillingSystem_skj.Business;
using System.Data;

namespace StellarBillingSystem_skj.Controllers
{
    [Authorize]
    public class StaffAdminController : Controller
    {
        private BillingContext _billingsoftware;
        private readonly IConfiguration _configuration;


        public StaffAdminController(BillingContext billingsoftware, IConfiguration configuration)
        {
            _billingsoftware = billingsoftware;
            _configuration = configuration;
        }

        private bool IsImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            // Check the file's MIME type to confirm it's an image
            if (file.ContentType.ToLower().StartsWith("image/"))
                return true;

            // You can add more checks here depending on your requirements

            return false;
        }




        public IActionResult GetIdProofImage(string staffId, string branchId)
        {
            var staffAdmin = _billingsoftware.SHStaffAdmin
                .FirstOrDefault(x => x.StaffID == staffId && x.BranchID == branchId && x.IsDelete == false);

            if (staffAdmin != null && staffAdmin.IdProofFile != null)
            {
                // Return the file with appropriate content type
                return File(staffAdmin.IdProofFile, "image/jpeg", "id-proof.jpg"); // Adjust filename and MIME type if necessary
            }

            return NotFound();  // Return 404 if the image is not found
        }



        public async Task<DataTable> AdditionalStaffFun(string branchID)
        {

            // Step 1: Perform the query
            var entities = await (from staff in _billingsoftware.SHStaffAdmin
                                  join rolname in _billingsoftware.SHrollType on staff.RolltypeID equals rolname.RollID into roll
                                  from r in roll.DefaultIfEmpty()
                                  where staff.BranchID == branchID && staff.IsDelete == false
                                  orderby staff.LastupdatedDate descending
                                  select new StaffAdminModel
                                  {
                                      StaffID = staff.StaffID,
                                      FullName = staff.FullName,
                                      RolltypeID = r.RollName,
                                      PhoneNumber = staff.PhoneNumber,
                                      EmailId = staff.EmailId
                                  }).ToListAsync();

            // Step 2: Convert to DataTable
            return BusinessClassBilling.ConvertToDataTableStaff(entities);


        }



        public async Task<IActionResult> StaffAdmin()
        {
            var model = new StaffAdminModel();

            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            SatffAdminBusinessClass Busbill = new SatffAdminBusinessClass(_billingsoftware, _configuration);
            ViewData["rollid"] = Busbill.RollAccessType(model.BranchID);
            ViewData["branchid"] = Busbill.Getbranch();

            var dataTable = await AdditionalStaffFun(model.BranchID);

            // Store the DataTable in ViewData for access in the view
            ViewData["StaffData"] = dataTable;

            return View(model);
        }

        // staff reg
        [HttpPost]
        public async Task<IActionResult> AddStaff(StaffAdminModel model, string buttontype, IFormFile imageFile)
        {

            SatffAdminBusinessClass Busbill = new SatffAdminBusinessClass(_billingsoftware, _configuration);
            ViewData["rollid"] = Busbill.RollAccessType(model.BranchID);
            ViewData["branchid"] = Busbill.Getbranch();




            if (buttontype == "Get")
            {
                if (TempData["BranchID"] != null)
                {
                    model.BranchID = TempData["BranchID"].ToString();
                    TempData.Keep("BranchID");
                }

                var getstaff = await _billingsoftware.SHStaffAdmin.FirstOrDefaultAsync(x => x.StaffID == model.StaffID && x.IsDelete == false && x.BranchID == model.BranchID);
                if (getstaff != null)
                {
                    model.Gender = string.IsNullOrEmpty(getstaff.Gender) ? string.Empty : getstaff.Gender;


                    var checkid = await _billingsoftware.SHStaffAdmin.FirstOrDefaultAsync(x => x.StaffID == model.StaffID && x.IsDelete == false && x.BranchID == model.BranchID && x.IdProofFile != null);

                    if (checkid != null)
                    {

                        // Prepare the image URL
                        ViewBag.ImageUrl = Url.Action("GetIdProofImage", new { staffId = getstaff.StaffID, branchId = getstaff.BranchID });
                        var dataTable1 = await AdditionalStaffFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["StaffData"] = dataTable1;
                        return View("StaffAdmin", getstaff);
                    }
                    else
                    {
                        var dataTable1 = await AdditionalStaffFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["StaffData"] = dataTable1;
                        return View("StaffAdmin", getstaff);

                    }
                }
                else
                {
                    StaffAdminModel par = new StaffAdminModel();
                    ViewBag.getMessage = "No Data found for this Staff ID";
                    var dataTable2 = await AdditionalStaffFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["StaffData"] = dataTable2;
                    return View("StaffAdmin", par);
                }

            }
            else if (buttontype == "Delete")
            {
                if (TempData["BranchID"] != null)
                {
                    model.BranchID = TempData["BranchID"].ToString();
                    TempData.Keep("BranchID");
                }

                var stafftodelete = await _billingsoftware.SHStaffAdmin.FindAsync(model.StaffID, model.BranchID);
                if (stafftodelete != null)
                {
                    if (stafftodelete.IsDelete)
                    {
                        ViewBag.ErrorMessage = "StaffID Already Deleted";
                        var dataTable3 = await AdditionalStaffFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["StaffData"] = dataTable3;
                        return View("StaffAdmin", model);
                    }

                    stafftodelete.IsDelete = true;
                    await _billingsoftware.SaveChangesAsync();

                    ViewBag.delMessage = "StaffID deleted successfully";
                    var dataTable4 = await AdditionalStaffFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["StaffData"] = dataTable4;

                    model = new StaffAdminModel();
                    return View("StaffAdmin", model);
                }
                else
                {
                    ViewBag.delnoMessage = "StaffID not found";
                    var dataTable5 = await AdditionalStaffFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["StaffData"] = dataTable5;
                    model = new StaffAdminModel();
                    return View("StaffAdmin", model);
                }

            }

            else if (buttontype == "DeleteRetrieve")
            {

                if (TempData["BranchID"] != null)
                {
                    model.BranchID = TempData["BranchID"].ToString();
                    TempData.Keep("BranchID");
                }


                var stafftoretrieve = await _billingsoftware.SHStaffAdmin.FindAsync(model.StaffID, model.BranchID);
                if (stafftoretrieve != null)
                {
                    if (stafftoretrieve.IsDelete == true)
                    {

                        stafftoretrieve.IsDelete = false;

                        await _billingsoftware.SaveChangesAsync();

                        model.StaffID = stafftoretrieve.StaffID;
                        model.FullName = stafftoretrieve.FullName;
                        model.ResourceTypeID = stafftoretrieve.ResourceTypeID;
                        model.FirstName = stafftoretrieve.FirstName;
                        model.LastName = stafftoretrieve.LastName;
                        model.Initial = stafftoretrieve.Initial;
                        model.Prefix = stafftoretrieve.Prefix;
                        model.PhoneNumber = stafftoretrieve.PhoneNumber;
                        model.DateofBirth = stafftoretrieve.DateofBirth;
                        model.Age = stafftoretrieve.Age;
                        model.Gender = stafftoretrieve.Gender;
                        model.Address1 = stafftoretrieve.Address1;
                        model.City = stafftoretrieve.City;
                        model.State = stafftoretrieve.State;
                        model.Pin = stafftoretrieve.Pin;
                        model.EmailId = stafftoretrieve.EmailId;
                        model.Nationality = stafftoretrieve.Nationality;
                        model.UserName = stafftoretrieve.UserName;
                        model.Password = stafftoretrieve.Password;
                        model.IdProofId = stafftoretrieve.IdProofId;
                        model.IdProofName = stafftoretrieve.IdProofName;
                        model.IdProofFile = stafftoretrieve.IdProofFile;
                        model.RolltypeID = stafftoretrieve.RolltypeID;

                        ViewBag.retMessage = "Deleted StaffID retrieved successfully";
                    }
                    else
                    {
                        ViewBag.retMessage = "StaffID  Already retrieved";
                    }
                }
                else
                {
                    ViewBag.noretMessage = "StaffID not found";
                }
                var dataTable6 = await AdditionalStaffFun(model.BranchID);

                // Store the DataTable in ViewData for access in the view
                ViewData["StaffData"] = dataTable6;
                return View("StaffAdmin", model);
            }



            var staffcheck = await _billingsoftware.SHStaffAdmin.FirstOrDefaultAsync(x => x.StaffID == model.StaffID && x.BranchID == model.BranchID && (x.UserName != model.UserName));



            if (staffcheck == null)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Validate that the uploaded file is an image (optional)
                    if (!IsImage(imageFile))
                    {
                        ModelState.AddModelError(string.Empty, "Uploaded file is not an image.");
                        var dataTable7 = await AdditionalStaffFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["StaffData"] = dataTable7;
                        return View("StaffAdmin", model);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        model.IdProofFile = memoryStream.ToArray();
                    }

                }
                else
                {
                    // If no new file is uploaded, retain the existing ID proof file
                    var mod = await _billingsoftware.SHStaffAdmin.FindAsync(model.StaffID, model.BranchID);
                    if (mod != null)
                    {
                        model.IdProofFile = mod.IdProofFile; // Retain existing file
                    }
                }



                var existingStaffAdmin = await _billingsoftware.SHStaffAdmin.FindAsync(model.StaffID, model.BranchID);


                if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
                {
                    ViewBag.validateMessage = "Username and Password are required.";
                    var dataTable8 = await AdditionalStaffFun(model.BranchID);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["StaffData"] = dataTable8;
                    return View("StaffAdmin", model);
                }

                if (existingStaffAdmin != null)
                {
                    if (existingStaffAdmin.IsDelete)
                    {
                        ViewBag.ErrorMessage = "Cannot update. Product is marked as deleted.";
                        var dataTable9 = await AdditionalStaffFun(model.BranchID);

                        // Store the DataTable in ViewData for access in the view
                        ViewData["StaffData"] = dataTable9;
                        return View("StaffAdmin", model);
                    }

                    existingStaffAdmin.StaffID = model.StaffID;
                    existingStaffAdmin.ResourceTypeID = model.ResourceTypeID;
                    existingStaffAdmin.BranchID = model.BranchID;
                    existingStaffAdmin.FirstName = model.FirstName;
                    existingStaffAdmin.LastName = model.LastName;
                    existingStaffAdmin.FullName = model.FullName;
                    existingStaffAdmin.Initial = model.Initial;
                    existingStaffAdmin.Prefix = model.Prefix;
                    existingStaffAdmin.Age = model.Age;
                    existingStaffAdmin.DateofBirth = model.DateofBirth;
                    existingStaffAdmin.EmailId = model.EmailId;
                    existingStaffAdmin.Address1 = model.Address1;
                    existingStaffAdmin.City = model.City;
                    existingStaffAdmin.State = model.State;
                    existingStaffAdmin.Pin = model.Pin;
                    existingStaffAdmin.PhoneNumber = model.PhoneNumber;
                    existingStaffAdmin.EmailId = model.EmailId;
                    existingStaffAdmin.Nationality = model.Nationality;
                    existingStaffAdmin.UserName = model.UserName;
                    existingStaffAdmin.Password = model.Password;
                    existingStaffAdmin.IdProofId = model.IdProofId;
                    existingStaffAdmin.IdProofName = model.IdProofName;
                    existingStaffAdmin.IdProofFile = model.IdProofFile;
                    existingStaffAdmin.LastupdatedDate = DateTime.ParseExact(Busbill.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                    existingStaffAdmin.LastupdatedUser = User.Claims.First().Value.ToString();
                    existingStaffAdmin.LastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    existingStaffAdmin.Gender = model.Gender;
                    existingStaffAdmin.RolltypeID = model.RolltypeID;

                    _billingsoftware.Entry(existingStaffAdmin).State = EntityState.Modified;

                }
                else
                {

                    model.LastupdatedDate = DateTime.ParseExact(Busbill.GetFormattedDateTime(), "dd/MM/yyyy HH:mm:ss", null);
                    model.LastupdatedUser = User.Claims.First().Value.ToString();
                    model.LastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    _billingsoftware.SHStaffAdmin.Add(model);
                }
            }
            else
            {

                ViewBag.ExistMessage = "Cannot Update Username";
                var dataTable10 = await AdditionalStaffFun(model.BranchID);

                // Store the DataTable in ViewData for access in the view
                ViewData["StaffData"] = dataTable10;
                return View("StaffAdmin", model);
            }
            await _billingsoftware.SaveChangesAsync();

            ViewBag.Message = "Saved Successfully";

            var dataTable = await AdditionalStaffFun(model.BranchID);

            // Store the DataTable in ViewData for access in the view
            ViewData["StaffData"] = dataTable;
            model = new StaffAdminModel();
            return View("StaffAdmin", model);


        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
