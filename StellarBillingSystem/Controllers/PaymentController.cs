using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    [Route("[controller]/[action]")]
    public class PaymentController : Controller
    {

        private BillingContext _billingsoftware;
        private readonly IConfiguration _configuration;


        public PaymentController(BillingContext billingsoftware, IConfiguration configuration)
        {
            _billingsoftware = billingsoftware;
            _configuration = configuration;
        }

        public IActionResult PaymentBilling()
        {
            PaymentTableViewModel obj = new PaymentTableViewModel();
            return View(obj);
        }




        public IActionResult UpdatePaymentDetails(string billID, string branchID, string billdate, string billValue)
        {
            string formattedBillDate = billdate;

            // Try parsing the billDate
            DateTime parsedBillDate;
            if (DateTime.TryParseExact(billdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedBillDate))
            {
                // If parsing succeeds, format the date correctly
                formattedBillDate = parsedBillDate.ToString("yyyy-MM-dd");
            }
            else
            {
                // If parsing fails, convert the billDate to DateTime and then format it
                DateTime tempDate;
                if (DateTime.TryParse(billdate, out tempDate))
                {
                    formattedBillDate = tempDate.ToString("yyyy-MM-dd");
                }
            }

            // Fetch the current balance from PaymentMaster
            var paymentDetails = _billingsoftware.SHPaymentMaster
                                                  .Where(p => p.BillId == billID && p.BranchID == branchID)
                                                  .Select(p => new
                                                  {
                                                      p.Balance
                                                  })
                                                  .FirstOrDefault();


            if (paymentDetails != null)
            {
                @ViewBag.Balance = paymentDetails.Balance;
            }
            else
            {
                @ViewBag.Balance = billValue;  // Default to 0 if Balance is null
              //  ViewBag.Message = "NotFound";
            }


            return View();
        }



        [HttpPost]
        public async Task<IActionResult> PaymentAction(PaymentTableViewModel model, string selectedSlotId,string buttonType, string billId, string branchID, string billDate, string billValue,string CloseBy,string CloseDate,string SalePayment,string SaleDate,string SaleRemark,string completelyClosing,string topUp,string sales,string ClosingStatus)
        {

            BusinessBillingSKJ business = new BusinessBillingSKJ(_billingsoftware, _configuration);


            if (TempData["BranchID"] != null)
            {
                model.BranchID = TempData["BranchID"].ToString();
                TempData.Keep("BranchID");
            }


            string formattedBillDate = billDate;

            // Try parsing the billDate
            DateTime parsedBillDate;
            if (DateTime.TryParseExact(billDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedBillDate))
            {
                // If parsing succeeds, format the date correctly
                formattedBillDate = parsedBillDate.ToString("yyyy-MM-dd");
            }
            else
            {
                // If parsing fails, convert the billDate to DateTime and then format it
                DateTime tempDate;
                if (DateTime.TryParse(billDate, out tempDate))
                {
                    formattedBillDate = tempDate.ToString("yyyy-MM-dd");
                }

            }


            //   model.StrBillvalue = BusinessClassCommon.getbalance(_billingsoftware, model.PaymentId, model.BillId,model.BranchID, model.BillDate,detailmodel.PaymentAmount);

            var paymentid = "pay_" + billId;


            if (billId == null && formattedBillDate == null)
            {
                TempData.Remove("BillValue"); // Optional: clear if not valid anymore
                ViewBag.Message = "BillID Not Found";
                var resultdel = UpdatePaymentDetails(billId, branchID, formattedBillDate, billValue);

                return View("PaymentBilling", model);
            }

          

            if (buttonType == "GetBill")
            {
                PaymentTableViewModel obj = new PaymentTableViewModel();

                var billDetails = _billingsoftware.Shbillmasterskj
                                             .Where(b => b.BillID == billId && b.BranchID == model.BranchID)
                                             .Select(b => new
                                             {
                                                 b.BillID,
                                                 b.BillDate,
                                                 b.TotalRepayValue,
                                                 b.closedBy,
                                                 b.ClosedDate,
                                                 b.ClosingStatus,
                                                 b.SaleDate,
                                                 b.SalePayment,
                                                 b.SaleRemark
                                             })
                                             .FirstOrDefault();

                // Fetch the payment details from the PaymentMaster and PaymentDetails tables based on the BillID
                var paymentDetails = (from pm in _billingsoftware.SHPaymentMaster
                                      join pd in _billingsoftware.SHPaymentDetails
                on pm.PaymentId equals pd.PaymentId
                                      where pm.BillId == billId && pm.BranchID == model.BranchID && pd.BranchID == model.BranchID
                                      select new
                                      {
                                          pd.PaymentId,
                                          pd.PaymentDiscription,
                                          pd.PaymentDate,
                                          pd.PaymentMode,
                                          pd.PaymentTransactionNumber,
                                          pd.PaymentAmount,
                                          pm.Balance // Fetch the balance from the PaymentMaster table
                                      }).ToList();

                // Check if bill details were found
                if (billDetails != null)
                {
                    obj.BillId = billDetails.BillID;
                    obj.BillDate = billDetails.BillDate;
                    obj.StrBillvalue = billDetails.TotalRepayValue.ToString();
                    obj.ClosedDate = billDetails.ClosedDate;
                    obj.closedBy = billDetails.closedBy;
                    obj.ClosingStatus = billDetails.ClosingStatus;
                    obj.SaleDate = billDetails.SaleDate;
                    obj.SalePayment = billDetails.SalePayment;
                    obj.SaleRemark = billDetails.SaleRemark;

                }
                else
                {
                    ViewBag.Message = "Bill details not found.";
                    return View("PaymentBilling", obj); // Early return if no bill found
                }

                // Check if payment details exist and assign balance accordingly
                if (paymentDetails != null && paymentDetails.Any())
                {
                    // If payments exist, use the balance from the PaymentMaster
                    ViewBag.Balance = paymentDetails.First().Balance;

                    obj.Viewpayment = paymentDetails.Select(pd => new PaymentDetailsModel
                    {
                        PaymentDate = pd.PaymentDate,
                        PaymentDiscription = pd.PaymentDiscription,
                        PaymentMode = pd.PaymentMode,
                        PaymentTransactionNumber = pd.PaymentTransactionNumber,
                        PaymentAmount = pd.PaymentAmount
                    }).ToList();
                }
                else
                {
                    // If no payment exists, set the balance to the bill's NetPrice
                    ViewBag.Balance = obj.StrBillvalue;
                    ViewBag.Total = obj.StrBillvalue;
                }

                model.StrBillvalue = BusinessBillingSKJ.getbalance(_billingsoftware, paymentid, billId, model.BranchID, model.Viewpayment?.LastOrDefault()?.PaymentAmount);

                // Store values in TempData to pass between requests
                TempData["BillID"] = obj.BillId;
                TempData["BillDate"] = DateTime.Parse(obj.BillDate).ToString("yyyy-MM-dd");
                TempData["BillValue"] = obj.StrBillvalue;
                TempData["Balance"] = ViewBag.Balance;
                TempData["BranchID"] = model.BranchID;

                ViewBag.Balance = model.StrBillvalue;
                 ViewBag.Total = obj.StrBillvalue;
                ViewBag.CloseBy = obj.closedBy;
                ViewBag.CloseDate = obj.ClosedDate;
                ViewBag.SalePayment = obj.SalePayment;
                ViewBag.SaleDate = obj.SaleDate;
                ViewBag.SaleRemark = obj.SaleRemark;
                ViewBag.ClosingStatus = obj.ClosingStatus;

                return View("PaymentBilling", obj);
            }

            if (buttonType == "DeletePayment")
            {
                //Delete Details from DB
                //Delete from database

                var Dbdelete = _billingsoftware.SHPaymentMaster.SingleOrDefault(x => x.BillId == billId && x.BranchID == model.BranchID && x.BillDate == formattedBillDate);

                if (Dbdelete != null)
                {

                    var selectedDBpayment = _billingsoftware.SHPaymentDetails.Where(x => x.PaymentId == paymentid && x.BranchID == model.BranchID).ToList();

                    if (selectedDBpayment.Count == 0)
                    {
                        ViewBag.Message = "Please enter Payment ID";
                        var resultdelm = UpdatePaymentDetails(billId, model.BranchID, formattedBillDate, billValue);
                        return View("PaymentBilling", model);
                    }

                    foreach (var item in selectedDBpayment)
                    {
                        _billingsoftware.SHPaymentDetails.Remove(item);
                        _billingsoftware.SaveChanges();
                    }


                    //Delete Master
                    var SelectedPayMas = _billingsoftware.SHPaymentMaster.SingleOrDefault(x => x.BillId == billId && x.BillDate == formattedBillDate && x.PaymentId == paymentid && x.BranchID == model.BranchID);

                    _billingsoftware.SHPaymentMaster.Remove(SelectedPayMas);
                    _billingsoftware.SaveChanges();
                    ViewBag.Message = "Payment Deleted Successfully";
                }
                else
                {
                    ViewBag.Message = "Payment Not Found";
                }
                var resultdel = UpdatePaymentDetails(billId, model.BranchID, formattedBillDate, billValue);


                //Code here for refresh model
                PaymentTableViewModel objnew = new PaymentTableViewModel();

                model = objnew;


                return View("PaymentBilling", model);
            }


            if (buttonType == "GetPayment")
            {
                var selectDBpayment = _billingsoftware.SHPaymentDetails.Where(x => x.PaymentId == model.PaymentId && x.BranchID == model.BranchID).ToList();


                var SelectPayMas = _billingsoftware.SHPaymentMaster.SingleOrDefault(x => x.BillId == model.BillId && x.PaymentId == model.PaymentId && x.BranchID == model.BranchID && x.BillDate == model.BillDate);

                if (SelectPayMas != null && selectDBpayment != null)
                {

                    if (model.Viewpayment == null)
                        model.Viewpayment = selectDBpayment;

                    model.BillDate = SelectPayMas.BillDate;
                    model.PaymentId = SelectPayMas.PaymentId;
                    model.BranchID = SelectPayMas.BranchID;
                    model.StrBillvalue = SelectPayMas.Balance;
                    model.BillId = SelectPayMas.BillId;

                    var exbilltotal = await _billingsoftware.SHbillmaster.Where(x => x.BillID == model.BillId && x.BillDate == model.BillDate && x.BranchID == model.BranchID).FirstOrDefaultAsync();
                    if (exbilltotal != null)
                        model.Balance = exbilltotal.NetPrice;


                }
                else
                {
                    ViewBag.Message = "Payment ID given is not available, Either it was belongs to different branch,enter correct Payment ID";
                }
            }
            if (buttonType == "DeletePaymentDetail")
            {

                if (string.IsNullOrEmpty(selectedSlotId))
                {
                    ViewBag.Message = "Please select a payment.";

                    var getbillvalue = await _billingsoftware.Shbillmasterskj.Where(x => x.BillID == billId && x.BranchID == model.BranchID).FirstOrDefaultAsync();
                    if (getbillvalue != null)
                       ViewBag.Total =getbillvalue.TotalRepayValue.ToString();
                    billValue = ViewBag.Total;

                    var resultdel = UpdatePaymentDetails(billId, model.BranchID, formattedBillDate, billValue);

                    return View("PaymentBilling", model);
                }


                //Delete from database
                var selectedDBpayment = _billingsoftware.SHPaymentDetails.SingleOrDefault(x => x.PaymentDiscription == selectedSlotId && x.PaymentId == paymentid && x.BranchID == model.BranchID);
                if (selectedDBpayment != null)
                {
                    _billingsoftware.SHPaymentDetails.Remove(selectedDBpayment);
                    _billingsoftware.SaveChanges();
                }

                //Delete from grid
                var selectedpayment = model.Viewpayment.SingleOrDefault(x => x.PaymentDiscription == selectedSlotId);
                if (selectedpayment != null)
                {
                    model.Viewpayment.Remove(selectedpayment);
                    ViewBag.Message = "Payment Detail Deleted Successfully";
                }
                else
                {
                    ViewBag.Message = "PaymentID Not Found";
                }

                model.StrBillvalue = BusinessBillingSKJ.getbalance(_billingsoftware, paymentid, billId, model.BranchID, model.Viewpayment?.LastOrDefault()?.PaymentAmount);

                var exbalance = _billingsoftware.SHPaymentMaster.Where(x => x.BillId == billId && x.BranchID == model.BranchID && x.PaymentId == paymentid && x.BillDate == formattedBillDate).FirstOrDefault();


                if (exbalance != null)
                {
                    exbalance.Balance = model.StrBillvalue;
                    _billingsoftware.Entry(exbalance).State = EntityState.Modified;
                    _billingsoftware.SaveChanges();
                }

                var exbilltotal = await _billingsoftware.Shbillmasterskj.Where(x => x.BillID == billId && x.BranchID == model.BranchID).FirstOrDefaultAsync();
                if (exbilltotal != null)
                    model.Balance = exbilltotal.TotalRepayValue.ToString();

                ViewBag.Total = exbilltotal.TotalRepayValue.ToString();

                var resultdelpay = UpdatePaymentDetails(billId, model.BranchID, formattedBillDate, billValue);
                return View("PaymentBilling", model);



            }



            if (buttonType == "Save")
            {

              /*  if(model.Viewpayment==null && model.Viewpayment.Any())
                {
                        ViewBag.Message = "Add Payment Cannot be Empty";

                        return View("PaymentBilling", model);  
                }*/

                double totalpayamount = 0.0;
                if (model.Viewpayment != null && model.Viewpayment.Any())
                {
                    foreach (var payment in model.Viewpayment)
                    {
                        if (!string.IsNullOrEmpty(payment.PaymentAmount))
                        {
                            if (double.TryParse(payment.PaymentAmount, out double amt))
                                totalpayamount += amt;
                        }
                    }
                

                    
                
                    foreach (var objdetail in model.Viewpayment)
                    {
                        var obpaydet = _billingsoftware.SHPaymentDetails
                            .Where(x => x.BranchID == model.BranchID
                                && x.PaymentDiscription == objdetail.PaymentDiscription
                                && x.PaymentId == paymentid)
                            .FirstOrDefault();

                        if (obpaydet != null)
                        {
                            obpaydet.BranchID = model.BranchID;
                            obpaydet.Lastupdateduser = User.Claims.First().Value.ToString();
                            obpaydet.Lastupdateddate = business.GetFormattedDateTime();
                            obpaydet.Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                            obpaydet.PaymentAmount = objdetail.PaymentAmount;
                            obpaydet.PaymentDate = objdetail.PaymentDate;
                            obpaydet.PaymentDiscription = objdetail.PaymentDiscription;
                            obpaydet.PaymentMode = objdetail.PaymentMode;
                            obpaydet.PaymentTransactionNumber = objdetail.PaymentTransactionNumber;

                            _billingsoftware.Entry(obpaydet).State = EntityState.Modified;
                        }
                        else
                        {
                            objdetail.PaymentId = paymentid;
                            objdetail.BranchID = model.BranchID;
                            objdetail.Lastupdateddate = business.GetFormattedDateTime();
                            objdetail.Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                            objdetail.Lastupdateduser = User.Claims.First().Value.ToString();

                            _billingsoftware.SHPaymentDetails.Add(objdetail);
                        }
                    }

                    _billingsoftware.SaveChanges(); // Only call if we added or modified records
                }

                var billAmount = _billingsoftware.Shbillmasterskj
                 .Where(x => x.BillID == billId && x.BranchID == model.BranchID)
                 .Select(x => x.TotalRepayValue)
                 .FirstOrDefault();

                // Check if total payment amount exceeds the bill amount
                if ((decimal)totalpayamount > billAmount)
                {
                    ViewBag.Message = HttpUtility.JavaScriptStringEncode($"Payment amount '{totalpayamount}' exceeds the total bill amount '{billValue}'");
                    var resultdel = UpdatePaymentDetails(billId, model.BranchID, formattedBillDate, billValue);

                    return View("PaymentBilling", model);
                }


                var objbillmaster = new PaymentMasterModel()
                {
                    BillDate = formattedBillDate,
                    PaymentId = paymentid,
                    BranchID = model.BranchID,
                    Balance = billValue,
                    BillId = billId,
                    Lastupdateddate = business.GetFormattedDateTime(),
                    Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Lastupdateduser = User.Claims.First().Value.ToString()

                };

                var objpaymas = _billingsoftware.SHPaymentMaster.Where(x => x.BillId == billId && x.BranchID == model.BranchID && x.PaymentId == paymentid && x.BillDate == formattedBillDate).FirstOrDefault();

                if (objpaymas != null)
                {
                    objpaymas.BranchID = model.BranchID;
                    objpaymas.Lastupdateddate = business.GetFormattedDateTime();
                    objpaymas.Lastupdateduser = User.Claims.First().Value.ToString();
                    objpaymas.Lastupdatedmachine = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    objpaymas.Balance = billValue;
                    objpaymas.BillDate = formattedBillDate;
                    objpaymas.BillId = billId;

                    _billingsoftware.Entry(objpaymas).State = EntityState.Modified;
                }
                else
                {
                    _billingsoftware.SHPaymentMaster.Add(objbillmaster);
                }

                _billingsoftware.SaveChanges();



                model.StrBillvalue = BusinessBillingSKJ.getbalance(_billingsoftware, paymentid, billId, model.BranchID, totalpayamount.ToString());

                var exbalance = _billingsoftware.SHPaymentMaster.FirstOrDefault(x => x.BillId == billId && x.BranchID == model.BranchID && x.PaymentId == paymentid && x.BillDate == formattedBillDate);


                if (exbalance != null)
                {
                    exbalance.Balance = model.StrBillvalue;
                    _billingsoftware.Entry(exbalance).State = EntityState.Modified;
                    _billingsoftware.SaveChanges();
                }

                var exbilltotal = await _billingsoftware.Shbillmasterskj.FirstOrDefaultAsync(x => x.BillID == billId && x.BranchID == model.BranchID);
                if (exbilltotal != null)
                    model.Balance = exbilltotal.TotalRepayValue.ToString();

                var updpayment = _billingsoftware.Shbillmasterskj.FirstOrDefault(x => x.BillID == billId && x.BranchID == model.BranchID);
                if (updpayment != null)
                {
                    updpayment.closedBy = CloseBy;
                    updpayment.ClosedDate = CloseDate;
                    updpayment.SalePayment = SalePayment;
                    updpayment.SaleDate = SaleDate;
                    updpayment.SaleRemark = SaleRemark;
                    updpayment.ClosingStatus = ClosingStatus;

                    _billingsoftware.SaveChanges();
                   
                }

                ViewBag.Message = "Payment Saved Successfully";

                ViewBag.Total = exbilltotal.TotalRepayValue.ToString();
                billValue = ViewBag.Total;

                var resultsav = UpdatePaymentDetails(billId, model.BranchID, formattedBillDate, billValue);


                return View("PaymentBilling", model);

            }
            if (buttonType == "AddPayment")
            {
                // Try to get existing bill first
                var exbill = await _billingsoftware.Shbillmasterskj
                    .FirstOrDefaultAsync(x => x.BillID == billId && x.BranchID == model.BranchID && x.IsDelete == false);

                if (exbill != null)
                {
                    BusinessBillingSKJ obj = new BusinessBillingSKJ(_billingsoftware, _configuration);
                  
                    PaymentDetailsModel objNewPayment = new PaymentDetailsModel
                    {
                        PaymentDiscription = obj.GeneratePaymentDescriptionreport(paymentid),
                        PaymentId = paymentid,
                        BranchID = model.BranchID
                    };

                    if (model.Viewpayment == null)
                        model.Viewpayment = new List<PaymentDetailsModel>();

                    model.Viewpayment.Add(objNewPayment);

                    model.Balance = exbill.TotalRepayValue.ToString();

                    var exbalance = _billingsoftware.SHPaymentMaster
                        .FirstOrDefault(x => x.BillId == billId && x.BranchID == model.BranchID && x.PaymentId == paymentid && x.BillDate == formattedBillDate);

                    if (exbalance != null)
                    {
                        model.StrBillvalue = exbalance.Balance;
                    }

                    _billingsoftware.SaveChanges();

                    ViewBag.Total = exbill.TotalRepayValue.ToString();
                    billValue=ViewBag.Total;

                    var result = UpdatePaymentDetails(billId, model.BranchID, formattedBillDate, billValue);
                }
                else
                {
                    // Bill not found: don't add to Viewpayment, just show a message
                    TempData.Remove("BillValue");
                    ViewBag.Message = "BillID Not Found";
                }
            }

            return View("PaymentBilling", model);
        }



            public IActionResult Index()
        {
            return View();
        }
    }
}
