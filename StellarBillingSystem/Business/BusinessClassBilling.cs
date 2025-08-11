using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StellarBillingSystem.Context;
using StellarBillingSystem.Models;
using System.Data;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace StellarBillingSystem.Business
{
    public class BusinessClassBilling
    {

        private readonly BillingContext _billingContext;
        private readonly IConfiguration _configuration;

        public BusinessClassBilling(BillingContext billingContext, IConfiguration configuration)
        {
            _billingContext = billingContext;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /*
              // Query to get all Product
                public DataTable GetProductData(string branchId)
                {
                    // Define the SQL query
                    string sqlQuery = "SELECT ProductID, ProductName, BarcodeId, Price FROM SHProductMaster WHERE BranchID = '"+ branchId +"' AND IsDelete = 0";

                    // Define the parameters for the SQL query
                    var parameters = new[]
                    {
                    new SqlParameter("@BranchID", branchId)
                };

                    // Use the DataTable method from BusinessClassCommon to execute the query
                    return BusinessClassCommon.DataTable(_billingContext, sqlQuery, parameters);
               }

        */


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

        // Format the date as ddMMyyyy hhmmss
        public string GetFormattedDateTime()
        {
            DateTime currentDateTime = GetCurrentDateTime();
            return currentDateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }




        public static DataTable ConvertToDataTableProductMaster(IEnumerable<ProductMatserModel> entities)
        {
            var dataTable = new DataTable();

            // Add columns
            dataTable.Columns.Add("ProductID", typeof(string));
            dataTable.Columns.Add("ProductName", typeof(string));
            dataTable.Columns.Add("BarcodeId", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));

            // Add rows
            foreach (var entity in entities)
            {
                dataTable.Rows.Add(entity.ProductID, entity.ProductName, entity.BarcodeId, entity.Price);
            }

            return dataTable;
        }

        public static DataTable ConvertToDataTableGodown(IEnumerable<GodownModel> entities)
        {
            var dataTable = new DataTable();

            // Add columns
            dataTable.Columns.Add("ProductID", typeof(string));
            dataTable.Columns.Add("NumberofStocks", typeof(string));

            // Add rows
            foreach (var entity in entities)
            {
                dataTable.Rows.Add(entity.ProductID, entity.NumberofStocks);
            }

            return dataTable;
        }

        public static DataTable ConvertToDataTableStaff(IEnumerable<StaffAdminModel> entities)
        {
            var dataTable = new DataTable();

            // Add columns
            dataTable.Columns.Add("StaffID", typeof(string));
            dataTable.Columns.Add("FullName", typeof(string));
            dataTable.Columns.Add("RolltypeID", typeof(string));
            dataTable.Columns.Add("PhoneNumber", typeof(string));
            dataTable.Columns.Add("EmailId", typeof(string));


            // Add rows
            foreach (var entity in entities)
            {
                dataTable.Rows.Add(entity.StaffID, entity.FullName, entity.RolltypeID, entity.PhoneNumber, entity.EmailId);
            }

            return dataTable;
        }




        //points Master
        public static DataTable convetToDataTablePointMaster(IEnumerable<PointsMasterModel> entities)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("NetPrice", typeof(string));
            dataTable.Columns.Add("NetPoints", typeof(string));
            foreach (var entity in entities)
            {
                dataTable.Rows.Add(entity.NetPrice, entity.NetPoints);

            }
            return dataTable;
        }


        public static DataTable convertToDataTableCategoryMaster(IEnumerable<CategoryMasterModel> entities)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("CategoryID", typeof(string));
            dataTable.Columns.Add("CategoryName", typeof(string));

            foreach (var entity in entities)
            {
                dataTable.Rows.Add(entity.CategoryID, entity.CategoryName);
            }

            return dataTable;
        }


        /*Product Dropdown*/

        public List<CategoryMasterModel> GetCatid(string BranchID)
        {
            var categoryid = (
                    from pr in _billingContext.SHCategoryMaster
                    where pr.BranchID == BranchID && pr.IsDelete == false
                    select new CategoryMasterModel
                    {
                        CategoryID = pr.CategoryID,
                        CategoryName = pr.CategoryName,
                    }).ToList();
            return categoryid;
        }


        public List<CategoryMasterModel> GetItemsFromDatabase(string branchid)
        {
            // Example data fetching from database
            return _billingContext.SHCategoryMaster.Where(x => x.BranchID == branchid).Select(x => new CategoryMasterModel
            {
                CategoryID = x.CategoryID,
                CategoryName = x.CategoryName
            }).ToList();
        }

        public ProductDropDownModel CreateProductDropDownModel(List<CategoryMasterModel> selectListItems, string selectedCategoryId, ProductMatserModel productModel)
        {
            return new ProductDropDownModel
            {
                Items = selectListItems,
                SelectedItemId = selectedCategoryId,
                ObjPro = productModel
            };
        }


        /*Points Reedem Details*/

        public List<CustomerMasterModel> GetCustomerid()
        {
            var pointreedemcustomerid = (
                from pr in _billingContext.SHCustomerMaster
                select new CustomerMasterModel
                {
                    CustomerID = pr.CustomerID,
                    CustomerName = pr.CustomerName,
                }).ToList();
            return pointreedemcustomerid;
        }

        /*Discount Category*/
        public List<CategoryMasterModel> GetcategoryID(string BranchID)
        {
            var discountcategoryid = (
                from pr in _billingContext.SHCategoryMaster
                where pr.BranchID == BranchID && pr.IsDelete == false
                select new CategoryMasterModel
                {
                    CategoryID = pr.CategoryID,
                    CategoryName = pr.CategoryName,

                }).ToList();
            return discountcategoryid;
        }



        /*Godown*/

        public List<ProductMatserModel> GetProductid(string BranchID)
        {
            var godownproductid = (
                from pr in _billingContext.SHProductMaster
                where pr.BranchID == BranchID && pr.IsDelete == false
                select new ProductMatserModel
                {
                    ProductID = pr.ProductID,
                    ProductName = pr.ProductName,
                }).ToList();
            return godownproductid;
        }



        public List<RackpartitionViewModel> GetRackview(string partitionID, string productID, string BranchID)
        {
            var result = (from p in _billingContext.SHRackPartionProduct
                          where p.PartitionID == partitionID && p.ProductID == productID && p.Isdelete == false && p.BranchID == BranchID
                          select new RackpartitionViewModel
                          {
                              PartitionID = p.PartitionID,
                              ProductID = p.ProductID,
                              Noofitems = p.Noofitems

                          }).ToList();
            return result;
        }



        public List<ResourceTypeMasterModel> GetResourceid(string BranchID)
        {
            var resoruseid = (
                    from pr in _billingContext.SHresourceType
                    where pr.BranchID == BranchID
                    select new ResourceTypeMasterModel
                    {
                        ResourceTypeID = pr.ResourceTypeID,
                        ResourceTypeName = pr.ResourceTypeName
                    }
               ).ToList();

            return resoruseid;
        }

        public List<RollTypeMaster> RollAccessType(string BranchID)
        {
            var rollid = (
                    from pr in _billingContext.SHrollType
                    where pr.BranchID == BranchID && pr.IsDelete == false
                    select new RollTypeMaster
                    {
                        RollID = pr.RollID,
                        RollName = pr.RollName
                    }
                ).ToList();

            return rollid;
        }

        public List<ScreenNameMasterModel> Screenname()
        {
            var screenname = (
                    from pr in _billingContext.SHScreenName
                    select new ScreenNameMasterModel
                    {
                        ScreenName = pr.ScreenName,
                    }
                ).ToList();

            return screenname;
        }


        public List<StaffAdminModel> GetStaffID(string BranchID)
        {
            var staffid = (
                    from pr in _billingContext.SHStaffAdmin
                    where pr.BranchID == BranchID && pr.IsDelete == false
                    select new StaffAdminModel
                    {
                        StaffID = pr.StaffID,
                        FullName = string.IsNullOrEmpty(pr.FullName) ? "Please enter Name in Staff Admin" : pr.FullName
                    }
                ).ToList();

            return staffid;
        }

        public List<ScreenMasterModel> GetScreenid(String BranchID)

        {
            var screenid = (
                        from pr in _billingContext.SHScreenMaster
                        where pr.BranchID == BranchID && pr.IsDelete == false
                        select new ScreenMasterModel
                        {
                            ScreenId = pr.ScreenId,

                            ScreenName = pr.ScreenName

                        }).ToList();
            return screenid;
        }

        public List<DiscountCategoryMasterModel> Getdiscountid(string BranchID)

        {
            var discountid = (
                        from pr in _billingContext.SHDiscountCategory
                        where pr.BranchID == BranchID && pr.IsDelete == false
                        select new DiscountCategoryMasterModel
                        {
                            CategoryID = pr.CategoryID,

                            DiscountPrice = pr.DiscountPrice

                        }).ToList();
            return discountid;
        }

        public List<BranchMasterModel> Getbranch()

        {
            var branchid = (
                        from pr in _billingContext.SHBranchMaster
                        where pr.IsDelete == false
                        select new BranchMasterModel
                        {
                            BracnchID = pr.BracnchID,

                            BranchName = pr.BranchName

                        }).ToList();
            return branchid;
        }

        public BranchMasterModel Getbranchinitial(string BranchID)

        {
            var branchidini = (
                        from pr in _billingContext.SHBranchMaster
                        where pr.IsDelete == false && pr.BracnchID == BranchID
                        select new BranchMasterModel
                        {
                            BranchInitial = pr.BranchInitial,
                            BranchName = pr.BranchName


                        }).FirstOrDefault();
            return branchidini;
        }

        public StaffAdminModel GetadminRT(string Username)

        {
            var adminRT = (
                        from pr in _billingContext.SHStaffAdmin
                        join ram in _billingContext.SHrollaccess on pr.StaffID equals ram.StaffID
                        join rol in _billingContext.SHrollType on ram.RollID equals rol.RollID
                        where pr.IsDelete == false && pr.UserName == Username && rol.RollName == "Admin" && ram.IsDelete == false && rol.IsDelete == false
                        select new StaffAdminModel
                        {
                            UserName = pr.UserName

                        }).FirstOrDefault();
            return adminRT;
        }

        public List<ProductMatserModel> Getproduct(string BranchID)
        {
            var productid = (
                        from product in _billingContext.SHProductMaster
                        join rack in _billingContext.SHGodown
                        on product.ProductID equals rack.ProductID
                        where product.BranchID == BranchID && rack.BranchID == BranchID && rack.IsDelete == false && product.IsDelete == false
                        select new { product, rack })
                  .AsEnumerable()
                  .Where(pr => int.Parse(pr.rack.NumberofStocks) > 0)
                  .Select(pr => pr.product)
                  .ToList();

            return productid;
        }


        public List<String> GetRoll(string userid, string BranchID)
        {

            var query = from sm in _billingContext.SHScreenMaster
                        join rac in _billingContext.SHRoleaccessModel on sm.ScreenName equals rac.ScreenID
                        join ram in _billingContext.SHrollaccess on rac.RollID equals ram.RollID
                        join sam in _billingContext.SHStaffAdmin on ram.StaffID equals sam.StaffID
                        join s in _billingContext.SHStaffAdmin on sam.StaffID equals s.StaffID
                        where rac.Authorized == "1" && sam.UserName == userid && sam.BranchID == BranchID && rac.BranchID == BranchID && ram.BranchID == BranchID && sm.BranchID == BranchID
                        select sm.ScreenName;

            var result = query.ToList();
            return result;
        }

        public List<String> Getadmin(string userid)
        {

            var query = from sm in _billingContext.SHScreenMaster
                        join rac in _billingContext.SHRoleaccessModel on sm.ScreenName equals rac.ScreenID
                        join ram in _billingContext.SHrollaccess on rac.RollID equals ram.RollID
                        join sam in _billingContext.SHStaffAdmin on ram.StaffID equals sam.StaffID
                        join s in _billingContext.SHStaffAdmin on sam.StaffID equals s.StaffID
                        where rac.Authorized == "1" && sam.UserName == userid
                        select sm.ScreenName;

            var result = query.ToList();
            return result;
        }





        public List<GenericReportModel> GetReportId()
        {
            var reportid = (
                    from pr in _billingContext.ShGenericReport
                    select new GenericReportModel
                    {
                        ReportName = pr.ReportName,
                    }
                ).ToList();

            return reportid;
        }

        public string GeneratePaymentDescription(string paymentId)
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
                document.ReplaceText("<<custname>>", pbillData.Rows[0]["CustomerName"].ToString());
                document.ReplaceText("<<custcont>>", pbillData.Rows[0]["CustomerNumber"].ToString());
                document.ReplaceText("<<billdate>>", pbillData.Rows[0]["BillDate"].ToString());
                document.ReplaceText("<<billno>>", pbillData.Rows[0]["BillID"].ToString());
                document.ReplaceText("<<custadd>>", pbillData.Rows[0]["CustomerAddress"].ToString());


                document.ReplaceText("<<total>>", pbillData.Rows[0]["MasterTotalprice"].ToString());
                document.ReplaceText("<<cgst>>", pbillData.Rows[0]["CGSTPercentage"].ToString());
                document.ReplaceText("<<sgst>>", pbillData.Rows[0]["SGSTPercentage"].ToString());



                document.ReplaceText("{Placeholder2}", "Dynamic Value 2");

                // Insert a new paragraph


                //Add a table

                int rowcount = 0;
                int temrowcount = 1;
                //Row data
                foreach (DataRow objRow in pbillData.Rows)
                {
                    document.ReplaceText("<<sno" + temrowcount.ToString() + ">>", temrowcount.ToString());
                    document.ReplaceText("<<description" + temrowcount.ToString() + ">>", pbillData.Rows[rowcount]["ProductName"].ToString());
                    document.ReplaceText("<<h" + temrowcount.ToString() + ">>", pbillData.Rows[rowcount]["SerialNumber"].ToString());
                    document.ReplaceText("<<q" + temrowcount.ToString() + ">>", pbillData.Rows[rowcount]["Quantity"].ToString());
                    document.ReplaceText("<<up" + temrowcount.ToString() + ">>", pbillData.Rows[rowcount]["Price"].ToString());
                    document.ReplaceText("<<amt" + temrowcount.ToString() + ">>", pbillData.Rows[rowcount]["NetPrice"].ToString());

                    rowcount++;
                    temrowcount++;
                }


                for (int emptycount = 1; emptycount <= 6; emptycount++)
                {
                    document.ReplaceText("<<sno" + emptycount.ToString() + ">>", string.Empty);
                    document.ReplaceText("<<description" + emptycount.ToString() + ">>", string.Empty);
                    document.ReplaceText("<<h" + emptycount.ToString() + ">>", string.Empty);
                    document.ReplaceText("<<q" + emptycount.ToString() + ">>", string.Empty);
                    document.ReplaceText("<<up" + emptycount.ToString() + ">>", string.Empty);
                    document.ReplaceText("<<amt" + emptycount.ToString() + ">>", string.Empty);


                }



                // Save the document to a MemoryStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    document.SaveAs(memoryStream);

                    // Convert the MemoryStream to a byte array
                    return memoryStream.ToArray();
                }

            }
            return null;
        }
        public byte[] PrintBillDetails(DataTable billDetails, string BranchID)
        {
            // Determine the template name based on the BranchID
            string templateName = BranchID == "Lee_Mobile" ? "BillTemplate Branch1.docx" : "BillTemplate Branch2.docx";

            // Combine the template path
            string templatePath = Path.Combine(templateName);

            return ModifyBillDoc(templatePath, billDetails);
        }


        public string GeneratePaymentDescriptionreport(string paymentId)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            return $"{paymentId}_{timestamp}";
        }

        //Procedure to print Bill details
        public byte[] ModifypaymentDoc(string pfilepath, DataTable pbillData)
        {
            // Path to your existing Word document
            string filePath = pfilepath;

            // Open the document
            using (var document = DocX.Load(filePath))
            {
                // Replace placeholders with dynamic data
                document.ReplaceText("<<custname>>", pbillData.Rows[0]["CustomerName"].ToString());
                document.ReplaceText("<<custnum>>", pbillData.Rows[0]["CustomerNumber"].ToString());
                document.ReplaceText("<<billdate>>", pbillData.Rows[0]["BillDate"].ToString());
                document.ReplaceText("<<billno>>", pbillData.Rows[0]["BillID"].ToString());
                document.ReplaceText("<<paymentno>>", pbillData.Rows[0]["PaymentId"].ToString());


                // Add a table
                var table = document.AddTable(pbillData.Rows.Count + 1, 5);

                table.Rows[0].Cells[0].Paragraphs[0].Append("Payment Description");
                table.Rows[0].Cells[1].Paragraphs[0].Append("Payment Mode");
                table.Rows[0].Cells[2].Paragraphs[0].Append("Payment Transaction Number");
                table.Rows[0].Cells[3].Paragraphs[0].Append("Payment Amount");
                table.Rows[0].Cells[4].Paragraphs[0].Append("Payment Date");


                int rowcount = 1;
                //Row data
                foreach (DataRow objRow in pbillData.Rows)
                {

                    table.Rows[rowcount].Cells[0].Paragraphs[0].Append(objRow["PaymentDiscription"].ToString());
                    table.Rows[rowcount].Cells[1].Paragraphs[0].Append(objRow["PaymentMode"].ToString());
                    table.Rows[rowcount].Cells[2].Paragraphs[0].Append(objRow["PaymentTransactionNumber"].ToString());
                    table.Rows[rowcount].Cells[3].Paragraphs[0].Append(objRow["PaymentAmount"].ToString());
                    table.Rows[rowcount].Cells[4].Paragraphs[0].Append(objRow["PaymentDate"].ToString());
                    rowcount++;
                }

                // Set font and font size for the entire table
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.Cells)
                    {
                        cell.Paragraphs[0].Font(new Font("Arial")).FontSize(14);
                    }
                }

                string searchText = "<<paymentnode>>";
                Paragraph paragraph = document.Paragraphs.FirstOrDefault(p => p.Text.Contains(searchText));

                if (paragraph != null)
                {
                    paragraph.InsertTableAfterSelf(table);
                }

                document.ReplaceText("<<paymentnode>>", String.Empty);

                // Save the document to a MemoryStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    document.SaveAs(memoryStream);

                    // Convert the MemoryStream to a byte array
                    return memoryStream.ToArray();
                }

            }
            return null;
        }
        public byte[] PrintpaymentDetails(DataTable billDetails)
        {
            return ModifypaymentDoc("..\\StellarBillingSystem\\Templates\\Payment Template.docx", billDetails);
        }




        public async Task<int> GetBalanceForBillAsync(string billId)
        {
            // Define the output parameter
            var outputParameter = new SqlParameter("@Balance", SqlDbType.Float)
            {
                Direction = ParameterDirection.Output
            };

            // Execute the SQL command with output parameter
            var sql = $"EXEC @Balance = dbo.GenerateBillID @BillId";
            await _billingContext.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@BillId", billId), outputParameter);


            if (outputParameter.Value != DBNull.Value)
            {
                return Convert.ToInt32(outputParameter.Value);
            }
            else
            {
                return 0;
            }

        }


        public async Task<(BillingMasterModel, string Validate)> CalculateBillingDetails(string billID, string billDate, string customerNumber, string discount, string cgstPercentage, string sgstPercentage, string BranchID)
        {

            var billingDetails = await _billingContext.SHbilldetails
                .Where(x => x.BillID == billID && x.BillDate == billDate && x.CustomerNumber == customerNumber && !x.IsDelete && x.BranchID == BranchID)
                .ToListAsync();

            var billMaster = await _billingContext.SHbillmaster
                .Where(x => x.BillID == billID && x.BillDate == billDate && !x.IsDelete && x.CustomerNumber == customerNumber && x.BranchID == BranchID)
                .FirstOrDefaultAsync();

            if ((billingDetails == null || !billingDetails.Any()) && billMaster == null)
            {
                return (null, "No billing details found.");
            }


            // Calculate total price
            decimal totalPrice = billingDetails.Sum(x => decimal.TryParse(x.NetPrice, out decimal price) ? price : 0);

            decimal discountDecimal = 0;
            decimal cgstPercentageDecimal = 0;
            decimal sgstPercentageDecimal = 0;

            if (billMaster != null)
            {

                // Convert percentage and discount from string to decimal onyl if the orginal value has change

                discountDecimal = decimal.TryParse(billMaster.TotalDiscount, out decimal discountValue) ? discountValue : 0;

                cgstPercentageDecimal = decimal.TryParse(billMaster.CGSTPercentage, out decimal cgstPercentageValue) ? cgstPercentageValue : 0;

                sgstPercentageDecimal = decimal.TryParse(billMaster.SGSTPercentage, out decimal sgstPercentageValue) ? sgstPercentageValue : 0;

            }




            // Calculate CGST and SGST amounts
            decimal cgstAmount = (totalPrice * cgstPercentageDecimal) / 100;
            decimal sgstAmount = (totalPrice * sgstPercentageDecimal) / 100;


            var billingmaster = await _billingContext.SHbillmaster
             .Where(x => x.BillID == billID && x.BillDate == billDate && x.CustomerNumber == customerNumber && !x.IsDelete).Select(x => x.Totalprice).FirstOrDefaultAsync();

            decimal billingMasterNetPrice = decimal.TryParse(billingmaster, out decimal NetPrice) ? NetPrice : totalPrice;

            // Calculate total after applying CGST and SGST
            decimal totalWithTaxes = billingMasterNetPrice + cgstAmount + sgstAmount;

            if (discountDecimal > totalWithTaxes)
            {
                return (null, "Discount cannot be greater than the total price.");
            }

            decimal netPrice = totalWithTaxes - discountDecimal;



            return (new BillingMasterModel
            {
                Totalprice = totalPrice.ToString("F2"),
                CGSTPercentageAmt = cgstAmount.ToString("F2"),
                SGSTPercentageAmt = sgstAmount.ToString("F2"),
                TotalDiscount = discountDecimal.ToString("F2"),
                NetPrice = netPrice.ToString("F2")
            }, null);
        }
    }

}
