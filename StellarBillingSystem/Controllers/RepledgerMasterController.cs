using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StellarBillingSystem.Context;
using StellarBillingSystem_skj.Models;
using System.Diagnostics;

namespace StellarBillingSystem_skj.Controllers
{
    [Authorize]
    public class RepledgerMasterController : Controller
    {

        private BillingContext _billingsoftware;
        private readonly IConfiguration _configuration;


        public RepledgerMasterController(BillingContext billingsoftware, IConfiguration configuration)
        {
            _billingsoftware = billingsoftware;
            _configuration = configuration;
        }


        public IActionResult RepledgerMaster()
        {
            RepledgerModel model = new RepledgerModel();
            return View("RepledgerMaster", model);
        }


        [HttpPost]
        public async Task<IActionResult> AddRepledger(RepledgerModel model)
        {
            try
            {

                var existingCustomer = await _billingsoftware.Shrepledgermodel.FirstOrDefaultAsync(x => (x.RepledgerPhoneNumber1 == model.RepledgerPhoneNumber1 || x.RepledgerName == model.RepledgerName) && x.IsDelete == false);

                if (existingCustomer != null)
                {
                    if (existingCustomer.IsDelete)
                    {
                        ViewBag.Message = "Cannot update. Customer Number is marked as deleted.";

                        return View("CustomerMaster", model);
                    }

                    existingCustomer.RepledgerName = model.RepledgerName;
                    existingCustomer.RepledgerPhoneNumber2 = model.RepledgerPhoneNumber2;
                    existingCustomer.RepledgerPhoneNumber1 = model.RepledgerPhoneNumber1;
                    existingCustomer.RepledgerAddress = model.RepledgerAddress;
                    existingCustomer.City = model.City;
                    existingCustomer.IsDelete = model.IsDelete;
                    existingCustomer.State = model.State;
                    existingCustomer.Country = model.Country;
                    existingCustomer.LastUpdatedDate = DateTime.Now.ToString();
                    existingCustomer.LastUpdatedUser = User.Claims.First().Value.ToString();
                    existingCustomer.LastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    //_billingsoftware.Entry(existingCustomer).State = EntityState.Modified;

                }
                else
                {
                    model.LastUpdatedDate = DateTime.Now.ToString();
                    model.LastUpdatedUser = User.Claims.First().Value.ToString();
                    model.LastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    _billingsoftware.Shrepledgermodel.Add(model);


                }

                await _billingsoftware.SaveChangesAsync();

                ViewBag.Message = "Saved Successfully";



                RepledgerModel mod = new RepledgerModel();

                return View("RepledgerMaster", mod);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Save error: {ex}");
                throw;
            }
        }

        public async Task<IActionResult> GetRepledger(RepledgerModel model)
        {


            var customer = await _billingsoftware.Shrepledgermodel.FirstOrDefaultAsync(x => (x.RepledgerPhoneNumber1 == model.RepledgerPhoneNumber1 || x.RepledgerName == model.RepledgerName) && x.IsDelete == false);

            if (customer == null)
            {
                model = new RepledgerModel();
                ViewBag.ErrorMessage = "Repledger not found";
                return View("RepledgerMaster", model); // Return an empty model if not found or deleted
            }
            else
            {
                return View("RepledgerMaster", customer);
            }
        }


        public async Task<IActionResult> DeleteRepledger(RepledgerModel model)
        {

            var existingCustomer = await _billingsoftware.Shrepledgermodel.FirstOrDefaultAsync(x => (x.RepledgerPhoneNumber1 == model.RepledgerPhoneNumber1 || x.RepledgerName == model.RepledgerName) && x.IsDelete == false);
            if (existingCustomer == null)
            {
                model = new RepledgerModel();
                ViewBag.ErrorMessage = "Repledger not found";
                return View("RepledgerMaster", model);
            }

            if (existingCustomer.IsDelete)
            {
                ViewBag.ErrorMessage = "Repledger Number Already Deleted";
                return View("CustomerMaster", model);
            }

            existingCustomer.IsDelete = true;
            existingCustomer.LastUpdatedDate = DateTime.Now.ToString();
            existingCustomer.LastUpdatedUser = User.Claims.First().Value.ToString();
            existingCustomer.LastUpdatedMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();


            // ✅ Save all at once to avoid context confusion and SQL syntax issues
            _billingsoftware.SaveChanges();

            ViewBag.Message = "Deleted Successfully";
            model = new RepledgerModel();

            return View("RepledgerMaster", model);
        }
    }
}
