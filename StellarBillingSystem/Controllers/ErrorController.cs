using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarBillingSystem.Context;
using StellarBillingSystem.Models;

namespace StellarBillingSystem.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        private BillingContext _billingContext;

        public ErrorController(BillingContext billingContext)
        {
            _billingContext = billingContext;
        }
        public IActionResult Error()
        {
            //Record error from context session
            WebErrorsModel webErrors = new WebErrorsModel();
            webErrors.ErrDateTime = DateTime.Now.ToString();
            webErrors.ErrodDesc = HttpContext.Session.GetString("ErrorMessage").ToString();
            webErrors.Username = User.Claims.First().Value.ToString();
            webErrors.ScreenName = HttpContext.Session.GetString("ScreenName").ToString();
            webErrors.MachineName = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            //Saving error into database
            _billingContext.SHWebErrors.Add(webErrors);
            _billingContext.SaveChangesAsync();

            return View(webErrors);
        }
    }
}
