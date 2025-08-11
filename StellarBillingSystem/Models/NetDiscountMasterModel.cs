public class NetDiscountMasterModel
{
    public NetDiscountMasterModel()
    {
    }

    private string netID;
    private String strNetDiscount;
    private String strLastUpdatedDate;
    private String strLastUpdatedUser;
    private String strLastUpdatedmachine;
    private string branchID;


    public string? NetDiscount { get => strNetDiscount; set => strNetDiscount = value; }
    public string? LastUpdatedDate { get => strLastUpdatedDate; set => strLastUpdatedDate = value; }
    public string? LastUpdatedUser { get => strLastUpdatedUser; set => strLastUpdatedUser = value; }
    public string? LastUpdatedmachine { get => strLastUpdatedmachine; set => strLastUpdatedmachine = value; }
    public string NetID { get => netID; set => netID = value; }
    public string BranchID { get => branchID; set => branchID = value; }
}
