using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data;
using System.Web;
using Microsoft.Data.SqlClient;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.CodeAnalysis.Elfie.Model;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.CodeAnalysis.FlowAnalysis;
using StellarBillingSystem.Context;
using Microsoft.IdentityModel.Tokens;
using Aspose.Words;
using System.Xml;


namespace StellarBillingSystem.Business
{
    public static class BusinessClassCommon
    {

        public static byte[] ConvertWordToPdf(byte[] wordBytes)
        {
            // Load the Word document from the byte array
            using (MemoryStream wordStream = new MemoryStream(wordBytes))
            {
                Document doc = new Document(wordStream);

                // Convert the document to PDF
                using (MemoryStream pdfStream = new MemoryStream())
                {
                    doc.Save(pdfStream, SaveFormat.Pdf);
                    return pdfStream.ToArray(); // Return the PDF as a byte array
                }
            }
        }

        public static String AddParameter(String FromDate, String ToDate, String ColName, String DateColumn, String Colvalue)
        {
            String Where = String.Empty;
            if (!FromDate.Equals(String.Empty) || !ToDate.Equals(String.Empty) || !Colvalue.Equals(String.Empty))
            {
                Where = " Where ";
            }

            if (!FromDate.Equals(string.Empty) && !ToDate.Equals(String.Empty))
                Where = Where + String.Format(" {0} between '{1}' and {2} ", DateColumn, FromDate, ToDate);
            else if (!FromDate.Equals(string.Empty))
                Where = Where + String.Format(" {0} = '{1}' ", DateColumn, FromDate);
            else if (!ToDate.Equals(string.Empty))
                Where = Where + String.Format(" {0} = '{1}' ", DateColumn, ToDate);

            if (Where.Contains("between") || Where.Contains("="))
                Where = Where + " and ";

            if (!Colvalue.Equals(string.Empty))
                Where = Where + String.Format(" {0} ='{1}' ", ColName, Colvalue);

            return Where;
        }



    public static DataTable DataTable(DbContext context, string sqlQuery,
                                        params DbParameter[] parameters)
        {


            // Your DbContext
            var dbContext = context;

            ///

            // Your SQL query
            string query = sqlQuery ;


            // Execute raw SQL query and retrieve data into a DataTable
            DataTable dataTable = new DataTable();
            using (var connection = dbContext.Database.GetDbConnection() as SqlConnection)
            {
                if (connection != null)
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dataTable.Load(reader);
                            }
                        }
                    }
                }
            }

            return dataTable;
        }

        public static DataTable DataTableReport(DbContext context, string sqlQuery, string Datecolumn, string Fromdate, string Todate,string GroupBy,string pbranchID
                                       ,params DbParameter[] parameters)
        {


            // Your DbContext
            var dbContext = context;

            ///

            // Your SQL query
            string query = sqlQuery;


            if (Fromdate!=null&Todate!=null)
            {
                sqlQuery = sqlQuery + " and " + Datecolumn + " between '" + Fromdate + "' and '" + Todate +"' ";
            }
            else if (Fromdate!=null)
            {
                sqlQuery = sqlQuery + " and " + Datecolumn + " >= '"+ Fromdate +"'";

            }
            else if (Todate!=null)
            {
                sqlQuery = sqlQuery + " and " + Datecolumn + " <= '" + Todate +"'";
            }

            if (pbranchID !=null && pbranchID !=string.Empty)
            {
                sqlQuery = sqlQuery + " and bh.BranchID ='" + pbranchID + "' "; 
            }
          
           if(GroupBy!= string.Empty)
            {
                sqlQuery =sqlQuery+ GroupBy + "";  
            }

            // Execute raw SQL query and retrieve data into a DataTable
            DataTable dataTable = new DataTable();
            using (var connection = dbContext.Database.GetDbConnection() as SqlConnection)
            {
                if (connection != null)
                {
                    connection.Open();
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dataTable.Load(reader);
                            }
                        }
                    }
                }
            }

            // Save the original read-only state of each column
            Dictionary<string, bool> originalReadOnlyState = new Dictionary<string, bool>();
            foreach (DataColumn column in dataTable.Columns)
            {
                originalReadOnlyState[column.ColumnName] = column.ReadOnly;
                column.ReadOnly = false; // Temporarily make the column writable
            }

            // Format decimal, double, and float columns to string with two decimal places
            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.DataType == typeof(decimal) || column.DataType == typeof(double) || column.DataType == typeof(float))
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row[column.ColumnName] != DBNull.Value)
                        {
                            row[column.ColumnName] = Convert.ToDecimal(row[column.ColumnName]).ToString("F2");
                        }
                    }
                }
            }

            // Restore the original read-only state of each column
            foreach (DataColumn column in dataTable.Columns)
            {
                column.ReadOnly = originalReadOnlyState[column.ColumnName];
            }

            return dataTable;
            
        }

        public static String getbalance(BillingContext billing, string strPayID, string pBillID, string strBranchid, string pbillDate, string pPaymentAmount)
        {
            var paymentList = billing.SHPaymentDetails.Where(x => x.PaymentId == strPayID && x.IsDelete == false && x.BranchID == strBranchid).Select(x => x.PaymentAmount).ToList();


            Double dblBalance = 0.0;

 

            foreach (var strpayment in paymentList)
            {
                if (!(String.IsNullOrEmpty(strpayment)))
                    dblBalance = dblBalance + Double.Parse(strpayment);
            }

            var billamount = billing.SHbillmaster.Where(x => x.BillID == pBillID && x.BillDate == pbillDate && x.BranchID == strBranchid).Select(x => x.NetPrice).FirstOrDefault();

            if (billamount != null)
                dblBalance = Double.Parse(billamount) - dblBalance;

            dblBalance = Math.Round(dblBalance, 2);

            return dblBalance.ToString();
        }

    }
}
