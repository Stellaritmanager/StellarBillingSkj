using System.ComponentModel.DataAnnotations;

namespace StellarBillingSystem.Models
{
    public class BillingMasterModel
    {
        public BillingMasterModel() { }

        private string billID;
        private string billDate;
        private string customerNumber;
        private string totalprice;
        private string totalDiscount;
        private string netPrice;
        private string cGSTPercentage;
        private string sGSTPercentage;
        private string cGSTPercentageAmt;
        private string sGSTPercentageAmt;
        private bool isDelete;
        private string lastupdateduser;
        private string lastupdateddate;
        private string lastupdatedmachine;
        private string branchID;
        private string billby;
        private bool billInsertion;
        private int id;

        public string BillID { get => billID; set => billID = value; }
        public string BillDate { get => billDate; set => billDate = value; }
        [MaxLength(10)]
        public string CustomerNumber { get => customerNumber; set => customerNumber = value; }
        public string? Totalprice { get => totalprice; set => totalprice = value; }
        public string? TotalDiscount { get => totalDiscount; set => totalDiscount = value; }
        public string? NetPrice { get => netPrice; set => netPrice = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }
        public string? Lastupdateduser { get => lastupdateduser; set => lastupdateduser = value; }
        public string? Lastupdateddate { get => lastupdateddate; set => lastupdateddate = value; }
        public string? Lastupdatedmachine { get => lastupdatedmachine; set => lastupdatedmachine = value; }
        [MaxLength(20)]
        public string BranchID { get => branchID; set => branchID = value; }
        public string? CGSTPercentage { get => cGSTPercentage; set => cGSTPercentage = value; }
        public string? SGSTPercentage { get => sGSTPercentage; set => sGSTPercentage = value; }
        public string? CGSTPercentageAmt { get => cGSTPercentageAmt; set => cGSTPercentageAmt = value; }
        public string? SGSTPercentageAmt { get => sGSTPercentageAmt; set => sGSTPercentageAmt = value; }
        public string? Billby { get => billby; set => billby = value; }
        public bool BillInsertion { get => billInsertion; set => billInsertion = value; }
        public int Id { get => id; set => id = value; }
    }
}
