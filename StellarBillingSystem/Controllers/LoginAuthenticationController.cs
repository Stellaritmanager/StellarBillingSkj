using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StellarBillingSystem.Context;
using StellarBillingSystem.Models;
using StellarBillingSystem_skj.Business;
using System.Security.Claims;

namespace StellarBillingSystem.Controllers
{

    public class LoginAuthenticationController : Controller
    {
        private BillingContext _billingContext;
        private readonly IConfiguration _configuration;

        public LoginAuthenticationController(BillingContext billingContext, IConfiguration configuration)
        {
            _billingContext = billingContext;
            _configuration = configuration;
        }


        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [Authorize]
        public IActionResult Administration()
        {

            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            SatffAdminBusinessClass Busbill = new SatffAdminBusinessClass(_billingContext, _configuration);

            ViewData["branchid"] = Busbill.Getbranch();
            return View();


        }






        public async Task<IActionResult> LogOut()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            return RedirectToAction("Login", "LoginAuthentication");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        [HttpPost]
        public async Task<IActionResult> Login(StaffAdminModel model)
        {
            var login = await _billingContext.SHStaffAdmin.FirstOrDefaultAsync(x => x.UserName == model.UserName);

            if (login != null)
            {
                if (login.Password == model.Password)
                {
                    login.UserName = model.UserName;

                    login.Password = model.Password;

                    List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, login.UserName),
                    new Claim("OtherProperties", "Example Role")
                };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme
                );
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                    SatffAdminBusinessClass Busreg = new SatffAdminBusinessClass(_billingContext, _configuration);



                    var branch = await _billingContext.SHStaffAdmin.FirstOrDefaultAsync(x => x.UserName == model.UserName);

                    var rolldetail = Busreg.GetRoll(model.UserName, branch.BranchID);


                    TempData["UserName"] = model.UserName;
                    TempData["BranchID"] = branch.BranchID;



                    // Set TempData with the filtered roll details
                    TempData["RollAccess"] = JsonConvert.SerializeObject(rolldetail);


                    var user = Busreg.GetadminRT(model.UserName);
                    if (user != null)
                    {

                        return RedirectToAction("Administration", "LoginAuthentication");

                    }

                    else
                    {
                        //Set branch name and branch intial 
                        var branchinitial = Busreg.Getbranchinitial(branch.BranchID);
                        TempData["BranchInitail"] = branchinitial.BranchInitial;
                        TempData["BranchName"] = branchinitial.BranchName;

                        ViewBag.BranchInitial = branchinitial;

                        return RedirectToAction("Index", "Home");
                    }


                }

            }
            ViewBag.Message = "Credentials are Incorrect ";

            return View();


        }



        [HttpGet]
        public IActionResult admin()
        {
            SatffAdminBusinessClass Busreg = new SatffAdminBusinessClass(_billingContext, _configuration);
            ViewData["branchid"] = Busreg.Getbranch();

            var userName = TempData["UserName"]?.ToString();
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "LoginAuthentication");
            }

            TempData["UserName"] = userName;
            return View();
        }

        public async Task<IActionResult> admin(AdminModel model)
        {

            var userName = TempData["UserName"]?.ToString();
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "LoginAuthentication");
            }

            SatffAdminBusinessClass Busreg = new SatffAdminBusinessClass(_billingContext, _configuration);


            ViewData["branchid"] = Busreg.Getbranch();



            var rolldetail = Busreg.Getadmin(userName);



            TempData["UserName"] = userName;
            TempData["BranchID"] = model.BranchID;

            var branchinitial = Busreg.Getbranchinitial(model.BranchID);
            TempData["BranchInitail"] = branchinitial.BranchInitial;
            TempData["BranchName"] = branchinitial.BranchName;

            ViewBag.BranchInitial = branchinitial;

            // Set TempData with the filtered roll details
            TempData["RollAccess"] = JsonConvert.SerializeObject(rolldetail);

            return RedirectToAction("Index", "Home");

        }
    }
}
