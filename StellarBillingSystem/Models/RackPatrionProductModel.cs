public class RackPatrionProductModel
{
    public RackPatrionProductModel() { }

    private String strPartitionID;
    private String strProductID;
    private string noofitems;
    private String strLastUpdatedDate;
    private String strLastUpdatedUser;
    private String strLastUpdatedmachine;
    private bool isdelete;
    private string branchID;

    public string PartitionID { get => strPartitionID; set => strPartitionID = value; }
    public string ProductID { get => strProductID; set => strProductID = value; }
    public string? LastUpdatedDate { get => strLastUpdatedDate; set => strLastUpdatedDate = value; }
    public string? LastUpdatedUser { get => strLastUpdatedUser; set => strLastUpdatedUser = value; }
    public string? LastUpdatedmachine { get => strLastUpdatedmachine; set => strLastUpdatedmachine = value; }
    public string? Noofitems { get => noofitems; set => noofitems = value; }
    public bool Isdelete { get => isdelete; set => isdelete = value; }
    public string BranchID { get => branchID; set => branchID = value; }
}

