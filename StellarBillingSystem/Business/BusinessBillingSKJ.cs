using StellarBillingSystem.Context;
using StellarBillingSystem.Models;
using System.Data;
using Xceed.Words.NET;
using Spire.Doc;
using System.IO;
using Spire.Pdf;
using StellarBillingSystem_skj.Models;

namespace StellarBillingSystem_skj.Business
{
    public class BusinessBillingSKJ
    {

        private readonly BillingContext _billingContext;
        private readonly IConfiguration _configuration;

        public BusinessBillingSKJ(BillingContext billingContext, IConfiguration configuration)
        {
            _billingContext = billingContext;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public List<CustomerMasterModel> getCustomerID(string BranchID)
        {
            var customerid = (
                    from pr in _billingContext.SHCustomerMaster
                    where pr.BranchID == BranchID && pr.IsDelete == false
                    select new CustomerMasterModel
                    {
                        CustomerID = pr.CustomerID,
                        MobileNumber = pr.MobileNumber,
                        CustomerName = pr.CustomerName,
                    }
                ).ToList();

            return customerid;
        }

        public List<RepledgerModel> getrepledgerID()
        {
            var repledgerid = (
                    from pr in _billingContext.Shrepledgermodel
                    where pr.IsDelete == false
                    select new RepledgerModel
                    {
                        RepledgerID = pr.RepledgerID,
                        RepledgerName = pr.RepledgerName,
                        RepledgerPhoneNumber1 = pr.RepledgerPhoneNumber1
                    }
                ).ToList();

            return repledgerid;
        }

        public List<CategoryMasterModel> getGoldtype(string BranchID)
        {
            var goldtype = (
                    from pr in _billingContext.SHCategoryMaster
                    where pr.BranchID == BranchID && pr.IsDelete == false
                    select new CategoryMasterModel
                    {
                        CategoryName = pr.CategoryName,
                        MarketRate = pr.MarketRate,
                        CategoryID = pr.CategoryID,
                    }
                ).ToList();

            return goldtype;
        }

        public DateTime GetCurrentDateTime()
        {
            string useIST = _configuration.GetValue<string>("DateTimeSettings:UseIST");

            DateTime currentDateTime = DateTime.Now;
            if (useIST.ToLower() == "yes")
            {
                // Return the current time
                return currentDateTime;
            }
            else
            {
                // Add 5 hours and 30 minutes to the current time
                return currentDateTime.AddHours(5).AddMinutes(30);
            }
        }

        public string GetFormattedDateTime()
        {
            DateTime currentDateTime = GetCurrentDateTime();
            return currentDateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static String getbalance(BillingContext billing, string strPayID, string pBillID, string strBranchid, string pPaymentAmount)
        {
            var paymentList = billing.SHPaymentDetails.Where(x => x.PaymentId == strPayID && x.IsDelete == false && x.BranchID == strBranchid).Select(x => x.PaymentAmount).ToList();


            Double dblBalance = 0.0;



            foreach (var strpayment in paymentList)
            {
                if (!(String.IsNullOrEmpty(strpayment)))
                    dblBalance = dblBalance + Double.Parse(strpayment);
            }

            decimal billamount = billing.Shbillmasterskj.Where(x => x.BillID == pBillID  && x.BranchID == strBranchid).Select(x => x.TotalRepayValue).FirstOrDefault();

            if (billamount != null)
                dblBalance = (double)(billamount - Convert.ToDecimal(dblBalance));

            dblBalance = Math.Round(dblBalance, 2);

            return dblBalance.ToString();
        }

        public string GeneratePaymentDescriptionreport(string paymentId)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            return $"{paymentId}_{timestamp}";
        }

        //Procedure to print Bill details
        public byte[] ModifyBillDoc(string pfilepath, DataTable pbillData)
        {
            // Path to your existing Word document
            string filePath = pfilepath;

            // Open the document
            using (var document = DocX.Load(filePath))
            {
                // Replace placeholders with dynamic data
               
                document.ReplaceText("<<billno>>", pbillData.Rows[0]["BillID"].ToString());
                document.ReplaceText("<<billdate>>", pbillData.Rows[0]["BillDate"].ToString());
                document.ReplaceText("<<totalrepay>>", pbillData.Rows[0]["TotalRepayValue"].ToString());
                document.ReplaceText("<<customername>>", pbillData.Rows[0]["CustomerName"].ToString());
                document.ReplaceText("<<customeraddress>>", pbillData.Rows[0]["Address"].ToString());
                document.ReplaceText("<<customernumber>>", pbillData.Rows[0]["MobileNumber"].ToString());


                document.ReplaceText("<<overallwe>>", pbillData.Rows[0]["OverallWeight"].ToString());
                document.ReplaceText("<<totalrepay>>", pbillData.Rows[0]["TotalRepayValue"].ToString());
                document.ReplaceText("<<ininterest>>", pbillData.Rows[0]["InitialInterest"].ToString());
                document.ReplaceText("<<posttenure>>", pbillData.Rows[0]["PostTenureInterest"].ToString());
                document.ReplaceText("<<totalinword>>", pbillData.Rows[0]["TotalvalueinWords"].ToString());



                document.ReplaceText("{Placeholder2}", "Dynamic Value 2");

                // Insert a new paragraph


                //Add a table

                int rowcount = 0;
                int temrowcount = 1;
                //Row data
                foreach (DataRow objRow in pbillData.Rows)
                {
                    document.ReplaceText("<<articlename" + temrowcount.ToString() + ">>", pbillData.Rows[rowcount]["ArticleName"].ToString());
                    document.ReplaceText("<<qty" + temrowcount.ToString() + ">>", pbillData.Rows[rowcount]["Quantity"].ToString());
                    document.ReplaceText("<<goldtype" + temrowcount.ToString() + ">>", pbillData.Rows[rowcount]["GoldType"].ToString());
                    document.ReplaceText("<<grosswe" + temrowcount.ToString() + ">>", pbillData.Rows[rowcount]["Grossweight"].ToString());
                    document.ReplaceText("<<rwe" + temrowcount.ToString() + ">>", pbillData.Rows[rowcount]["Reducedweight"].ToString());
                    document.ReplaceText("<<nwe" + temrowcount.ToString() + ">>", pbillData.Rows[rowcount]["Netweight"].ToString());

                    rowcount++;
                    temrowcount++;
                }


                for (int emptycount = 1; emptycount <= 6; emptycount++)
                {
                   
                    document.ReplaceText("<<articlename" + emptycount.ToString() + ">>", string.Empty);
                    document.ReplaceText("<<qty" + emptycount.ToString() + ">>", string.Empty);
                    document.ReplaceText("<<goldtype" + emptycount.ToString() + ">>", string.Empty);
                    document.ReplaceText("<<grosswe" + emptycount.ToString() + ">>", string.Empty);
                    document.ReplaceText("<<rwe" + emptycount.ToString() + ">>", string.Empty);
                    document.ReplaceText("<<nwe" + emptycount.ToString() + ">>", string.Empty);

                }

                using (MemoryStream ms = new MemoryStream())
                {
                    document.SaveAs(ms);
                    return ms.ToArray(); // ✅ Important: return docx bytes
                }

            }
            return null;
        }
        public byte[] PrintBillDetails(DataTable billDetails, string BranchID)
        {
            string templateName = BranchID == "Lee_Mobile"
                ? "BillSKJTemplate Branch1.docx"
                : "BillTemplate Branch2.docx";

            string templatePath = Path.Combine(templateName);

            // Step 1: Generate filled .docx content
            byte[] docxBytes = ModifyBillDoc(templatePath, billDetails);

            // Step 2: Convert the .docx content to PDF using Spire.Doc
            using (MemoryStream docxStream = new MemoryStream(docxBytes))
            {
                Document spireDoc = new Document();
                spireDoc.LoadFromStream(docxStream, Spire.Doc.FileFormat.Docx);

                using (MemoryStream pdfStream = new MemoryStream())
                {
                    spireDoc.SaveToStream(pdfStream, Spire.Doc.FileFormat.PDF);
                    return pdfStream.ToArray(); // Return as PDF
                }
            }
        }
    }
}
