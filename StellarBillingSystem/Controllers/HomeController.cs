using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StellarBillingSystem.Business;
using StellarBillingSystem.Context;
using StellarBillingSystem.Models;
using System.Data;



namespace StellarBillingSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private BillingContext _billingContext;
        private readonly IConfiguration _configuration;

        public HomeController(BillingContext billingContext, IConfiguration configuration)
        {
            _billingContext = billingContext;
            _configuration = configuration;
        }


        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(string DashBoard = null, string fromDate = null, string toDate = null, string GroupBy = null)
        {

            string branchId = string.Empty; ;
            if (TempData["BranchID"] != null)
            {
                branchId = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }



            string salesMessage = GetSalesComparison();
            decimal dailySales = GetDailySales();
            decimal dailyPayments = GetDailyPayments();

            ViewBag.SalesMessage = salesMessage;
            ViewBag.DailySales = dailySales.ToString("F2");
            ViewBag.DailyPayments = dailyPayments.ToString("F2");



            /* BusinessClassBilling business = new BusinessClassBilling(_billingContext);
             ViewData["reportid"] = business.GetReportId();

             var reportQuery = (from rep in _billingContext.ShGenericReport
                                where rep.IsDashboard == true
                                select new GenericReportModel
                                {
                                    ReportName = rep.ReportName,
                                    ReportQuery = rep.ReportQuery,
                                    ReportDescription = rep.ReportDescription,
                                    Datecolumn = rep.Datecolumn,
                                    GroupBy = rep.GroupBy
                                }).FirstOrDefault();

             if (reportQuery != null)
             {
                 var query = BusinessClassCommon.DataTableReport(_billingContext, reportQuery.ReportQuery, reportQuery.Datecolumn, fromDate, toDate, reportQuery.GroupBy, branchId);
                 ViewBag.Reportname = reportQuery.ReportName;
                 return View(query);
             }*/


            return View();
        }


        private string GetSalesComparison()
        {
            string branchId = string.Empty;

            if (TempData["BranchID"] != null)
            {
                branchId = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            string result = string.Empty;

            string connectionString = _configuration.GetConnectionString("BillingDBConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT dbo.CompareDailySales(@BranchID)", conn))
                {
                    cmd.Parameters.AddWithValue("@BranchID", branchId);
                    result = cmd.ExecuteScalar().ToString();
                }
            }

            return result;
        }

        private decimal GetDailySales()
        {


            string branchId = string.Empty;

            if (TempData["BranchID"] != null)
            {
                branchId = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            decimal result = 0;



            string connectionString = _configuration.GetConnectionString("BillingDBConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT dbo.GetDailySales(@BranchID)", conn))
                {
                    cmd.Parameters.AddWithValue("@BranchID", branchId);
                    var dbResult = cmd.ExecuteScalar();
                    if (dbResult != DBNull.Value)
                    {
                        result = Convert.ToDecimal(dbResult);
                    }
                }
            }

            return result;
        }

        private decimal GetDailyPayments()
        {


            string branchId = string.Empty;

            if (TempData["BranchID"] != null)
            {
                branchId = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            decimal result = 0;

            string connectionString = _configuration.GetConnectionString("BillingDBConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT dbo.GetDailyPayments(@BranchID)", conn))
                {
                    cmd.Parameters.AddWithValue("@BranchID", branchId);
                    var dbResult = cmd.ExecuteScalar();
                    if (dbResult != DBNull.Value)
                    {
                        result = Convert.ToDecimal(dbResult);
                    }
                }
            }

            return result;
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Administration()
        {
            return View();
        }



        public IActionResult RedirectToReports(string DashBoard = null, string fromDate = null, string toDate = null, string GroupBy = null)
        {


            string salesMessage = GetSalesComparison();
            decimal dailySales = GetDailySales();
            decimal dailyPayments = GetDailyPayments();

            ViewBag.SalesMessage = salesMessage;
            ViewBag.DailySales = dailySales.ToString("F2");
            ViewBag.DailyPayments = dailyPayments.ToString("F2");


            string branchId = string.Empty; ;
            if (TempData["BranchID"] != null)
            {
                branchId = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }

            BusinessClassBilling business = new BusinessClassBilling(_billingContext, _configuration);
            ViewData["reportid"] = business.GetReportId();

            var reportQuery = (from rep in _billingContext.ShGenericReport
                               where rep.IsDashboard == true
                               select new GenericReportModel
                               {
                                   ReportName = rep.ReportName,
                                   ReportQuery = rep.ReportQuery,
                                   ReportDescription = rep.ReportDescription,
                                   Datecolumn = rep.Datecolumn,
                                   GroupBy = rep.GroupBy
                               }).FirstOrDefault();

            if (reportQuery != null)
            {
                var query = BusinessClassCommon.DataTableReport(_billingContext, reportQuery.ReportQuery, reportQuery.Datecolumn, fromDate, toDate, reportQuery.GroupBy, branchId);
                ViewBag.Reportname = reportQuery.ReportName;

                return View("Index", query);
            }

            return View("Index");
        }
    }

}

