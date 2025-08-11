namespace StellarBillingSystem.Models
{
    public class ProductDropDownModel
    {
        public ProductDropDownModel() { }

        private String selectedItemId;
        private List<CategoryMasterModel> items;
        private ProductMatserModel objPro;



        public ProductMatserModel ObjPro { get => objPro; set => objPro = value; }
        public string? SelectedItemId { get => selectedItemId; set => selectedItemId = value; }
        public List<CategoryMasterModel> Items { get => items; set => items = value; }
    }
}
