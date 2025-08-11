public class BilingSysytemModel
{
    public BilingSysytemModel()
    {
    }

    private String strBillID;
    private String strCustomerName;
    private String strDate;
    private String strCustomerNumber;
    private String strItems;
    private String strRate;
    private String strQuantity;
    private String strDiscount;
    private String strTax;
    private String strDiscountPrice;
    private String strTotalAmount;
    private String strPointsNumber;
    private String strVoucherNumber;
    private String strCategoryBasedDiscount;
    private String strLastUpdatedDate;
    private String strLastUpdatedUser;
    private String strLastUpdatedmachine;
    private string branchID;

    public string BillID { get => strBillID; set => strBillID = value; }
    public string? CustomerName { get => strCustomerName; set => strCustomerName = value; }
    public string? Date { get => strDate; set => strDate = value; }
    public string? CustomerNumber { get => strCustomerNumber; set => strCustomerNumber = value; }
    public string? Items { get => strItems; set => strItems = value; }
    public string? Rate { get => strRate; set => strRate = value; }
    public string? Quantity { get => strQuantity; set => strQuantity = value; }
    public string? Discount { get => strDiscount; set => strDiscount = value; }
    public string? Tax { get => strTax; set => strTax = value; }
    public string? DiscountPrice { get => strDiscountPrice; set => strDiscountPrice = value; }
    public string? TotalAmount { get => strTotalAmount; set => strTotalAmount = value; }
    public string? PointsNumber { get => strPointsNumber; set => strPointsNumber = value; }
    public string? VoucherNumber { get => strVoucherNumber; set => strVoucherNumber = value; }
    public string? CategoryBasedDiscount { get => strCategoryBasedDiscount; set => strCategoryBasedDiscount = value; }
    public string? LastUpdatedDate { get => strLastUpdatedDate; set => strLastUpdatedDate = value; }
    public string? LastUpdatedUser { get => strLastUpdatedUser; set => strLastUpdatedUser = value; }
    public string? LastUpdatedmachine { get => strLastUpdatedmachine; set => strLastUpdatedmachine = value; }
    public string BranchID { get => branchID; set => branchID = value; }
}
