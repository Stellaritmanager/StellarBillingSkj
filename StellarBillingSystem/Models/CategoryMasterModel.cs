public class CategoryMasterModel
{
    public CategoryMasterModel() { }

    private String strCategoryID;
    private String strCategoryName;
    private bool strIsDelete;
    private DateTime strLastUpdatedDate;
    private String strLastUpdatedUser;
    private String strLastUpdatedmachine;
    private string branchID;
    private string marketRate;
    private long id;

    public string CategoryID { get => strCategoryID; set => strCategoryID = value; }
    public string? CategoryName { get => strCategoryName; set => strCategoryName = value; }

    public string? LastUpdatedUser { get => strLastUpdatedUser; set => strLastUpdatedUser = value; }
    public string? LastUpdatedmachine { get => strLastUpdatedmachine; set => strLastUpdatedmachine = value; }
    public bool IsDelete { get => strIsDelete; set => strIsDelete = value; }
    public string BranchID { get => branchID; set => branchID = value; }
    public DateTime LastUpdatedDate { get => strLastUpdatedDate; set => strLastUpdatedDate = value; }
    public long Id { get => id; set => id = value; }
    public string MarketRate { get => marketRate; set => marketRate = value; }
}


