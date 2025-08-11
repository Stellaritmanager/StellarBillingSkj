
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarBillingSystem.Business;
using StellarBillingSystem.Context;
using StellarBillingSystem.Models;

namespace StellarBillingSystem.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {

        private BillingContext _billingContext;
        private readonly IConfiguration _configuration;

        public ReportsController(BillingContext billingContext, IConfiguration configuration)
        {
            _billingContext = billingContext;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult GetReports(String inputValue, string fromDate, string toDate, string GroupBy)
        {
            BusinessClassBilling business = new BusinessClassBilling(_billingContext, _configuration);
            ViewData["reportid"] = business.GetReportId();

            string branchId = string.Empty; ;
            if (TempData["BranchID"] != null)
            {
                branchId = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            var reportQuery = (from rep in _billingContext.ShGenericReport
                               where (rep.ReportName == inputValue)
                               select new GenericReportModel
                               {
                                   ReportName = rep.ReportName,
                                   ReportQuery = rep.ReportQuery,
                                   ReportDescription = rep.ReportDescription,
                                   Datecolumn = rep.Datecolumn,
                                   GroupBy = rep.GroupBy
                               }).First();

            if (reportQuery == null)
            {
                ViewBag.Message = "Not Found";
                return View("Reports");
            }

            var query = BusinessClassCommon.DataTableReport(_billingContext, reportQuery.ReportQuery, reportQuery.Datecolumn, fromDate, toDate, reportQuery.GroupBy, branchId);



            ViewBag.Reportname = reportQuery.ReportName;


            return View("Reports", query);
        }


        public IActionResult Reports()
        {
            BusinessClassBilling business = new BusinessClassBilling(_billingContext, _configuration);
            ViewData["reportid"] = business.GetReportId();


            return View();
        }


    }
}
