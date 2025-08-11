namespace StellarBillingSystem.Models
{
    public class PaymentDetailsModel
    {
        public PaymentDetailsModel()
        {
        }

        private String strPaymentId;
        private String strPaymentDiscription;
        private String strPaymentMode;
        private String strPaymentTransactionNumber;
        private String strPaymentAmount;
        private String strPaymentDate;
        private bool isDelete;
        private String lastupdateduser;
        private String lastupdateddate;
        private String lastupdatedmachine;
        private string branchID;

        public string PaymentId { get => strPaymentId; set => strPaymentId = value; }
        public string PaymentDiscription { get => strPaymentDiscription; set => strPaymentDiscription = value; }
        public string? PaymentMode { get => strPaymentMode; set => strPaymentMode = value; }
        public string? PaymentTransactionNumber { get => strPaymentTransactionNumber; set => strPaymentTransactionNumber = value; }
        public string? PaymentAmount { get => strPaymentAmount; set => strPaymentAmount = value; }
        public string? PaymentDate { get => strPaymentDate; set => strPaymentDate = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }
        public string? Lastupdateduser { get => lastupdateduser; set => lastupdateduser = value; }
        public string? Lastupdateddate { get => lastupdateddate; set => lastupdateddate = value; }
        public string? Lastupdatedmachine { get => lastupdatedmachine; set => lastupdatedmachine = value; }
        public string BranchID { get => branchID; set => branchID = value; }
    }
}
