namespace StellarBillingSystem.Models
{
    public class ReedemHistoryModel
    {

        public ReedemHistoryModel()
        {
        }

        private String customerNumber;
        private String dateOfReedem;
        private String reedemPoints;
        private String strPaymentId;
        private String strPaymentDate;
        private String strBillId;
        private bool isDelete;
        private String lastupdateduser;
        private String lastupdateddate;
        private String lastupdatedmachine;
        private string branchID;

        public string CustomerNumber { get => customerNumber; set => customerNumber = value; }
        public string DateOfReedem { get => dateOfReedem; set => dateOfReedem = value; }
        public string? ReedemPoints { get => reedemPoints; set => reedemPoints = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }
        public string? Lastupdateduser { get => lastupdateduser; set => lastupdateduser = value; }
        public string? Lastupdateddate { get => lastupdateddate; set => lastupdateddate = value; }
        public string? Lastupdatedmachine { get => lastupdatedmachine; set => lastupdatedmachine = value; }
        public string? PaymentId { get => strPaymentId; set => strPaymentId = value; }
        public string? PaymentDate { get => strPaymentDate; set => strPaymentDate = value; }
        public string? BillId { get => strBillId; set => strBillId = value; }
        public string BranchID { get => branchID; set => branchID = value; }
    }


}
