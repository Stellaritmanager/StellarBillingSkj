namespace StellarBillingSystem.Models
{
    public class ProductSelectModel
    {
        public ProductSelectModel() { }

        private string productID;
        private string barcodeID;
        private string quantity;
        private string branchID;
        private List<ProductMatserModel> viewproductlist;

        public string ProductID { get => productID; set => productID = value; }
        public string BarcodeID { get => barcodeID; set => barcodeID = value; }
        public string Quantity { get => quantity; set => quantity = value; }
        public List<ProductMatserModel> Viewproductlist { get => viewproductlist; set => viewproductlist = value; }
        public string BranchID { get => branchID; set => branchID = value; }
    }
}
