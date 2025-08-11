namespace StellarBillingSystem.Models
{
    public class BillTableModel
    {
        public BillTableModel()
        {
        }
        private string billID;
        private string billDate;
        private string productID;
        private string productName;
        private string discount;
        private string price;
        private string quantity;
        private string netPrice;
        private string totalprice;
        private string totalDiscount;
        private string customerNumber;
        private bool isDelete;
        private string lastupdateduser;
        private string lastupdateddate;
        private string lastupdatedmachine;
        private string branchID;

        public string BillID { get => billID; set => billID = value; }
        public string BillDate { get => billDate; set => billDate = value; }
        public string Discount { get => discount; set => discount = value; }
        public string Price { get => price; set => price = value; }
        public string Quantity { get => quantity; set => quantity = value; }
        public string NetPrice { get => netPrice; set => netPrice = value; }
        public string Totalprice { get => totalprice; set => totalprice = value; }
        public string TotalDiscount { get => totalDiscount; set => totalDiscount = value; }
        public string CustomerNumber { get => customerNumber; set => customerNumber = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }
        public string Lastupdateduser { get => lastupdateduser; set => lastupdateduser = value; }
        public string Lastupdateddate { get => lastupdateddate; set => lastupdateddate = value; }
        public string Lastupdatedmachine { get => lastupdatedmachine; set => lastupdatedmachine = value; }

        public string ProductName { get => productName; set => productName = value; }
        public string ProductID { get => productID; set => productID = value; }
        public string BranchID { get => branchID; set => branchID = value; }
    }
}
