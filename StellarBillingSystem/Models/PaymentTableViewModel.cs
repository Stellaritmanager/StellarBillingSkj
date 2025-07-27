namespace StellarBillingSystem.Models
{
    public class PaymentTableViewModel
    {
        public PaymentTableViewModel() 
        {
        }

        private String strBillId;
        private String strPaymentId;
        private String strBalance;
        private String strBillDate;
        private bool strIsDelete;
        private string branchID;
        private List<PaymentDetailsModel> viewpayment;
        private String strBillvalue;

       

        public string BillId { get => strBillId; set => strBillId = value; }
        public string PaymentId { get => strPaymentId; set => strPaymentId = value; }
        public string? Balance { get => strBalance; set => strBalance = value; }
        public bool IsDelete { get => strIsDelete; set => strIsDelete = value; }
        public List<PaymentDetailsModel> Viewpayment { get => viewpayment; set => viewpayment = value; }
        public string BillDate { get => strBillDate; set => strBillDate = value; }
        public string BranchID { get => branchID; set => branchID = value; }
        public string StrBillvalue { get => strBillvalue; set => strBillvalue = value; }

        public string? ClosedDate { get; set; }

        public string? closedBy { get; set; }

        public string? ClosingStatus { get; set; }

        public string? SaleDate { get; set; }

        public string? SalePayment { get; set; }

        public string? SaleRemark { get; set; }
    }
}
