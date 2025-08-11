namespace StellarBillingSystem.Models
{
    public class PaymentMasterModel
    {

        public PaymentMasterModel()
        {
        }
        private String strBillId;
        private String strPaymentId;
        private String strBalance;
        private String strBillDate;
        private bool strIsDelete;
        private String lastupdateduser;
        private String lastupdateddate;
        private String lastupdatedmachine;
        private string branchID;

        public string BillId { get => strBillId; set => strBillId = value; }
        public string PaymentId { get => strPaymentId; set => strPaymentId = value; }
        public string? Balance { get => strBalance; set => strBalance = value; }
        public bool IsDelete { get => strIsDelete; set => strIsDelete = value; }
        public string? Lastupdateduser { get => lastupdateduser; set => lastupdateduser = value; }
        public string? Lastupdateddate { get => lastupdateddate; set => lastupdateddate = value; }
        public string? Lastupdatedmachine { get => lastupdatedmachine; set => lastupdatedmachine = value; }
        public string? BillDate { get => strBillDate; set => strBillDate = value; }
        public string BranchID { get => branchID; set => branchID = value; }
    }
}
