public class PointsMasterModel
{

    public PointsMasterModel()
    {

    }

    private String strPointsID;
    private String strNetPrice;
    private String strNetpoints;
    private String strLastUpdatedDate;
    private String strLastUpdatedUser;
    private String strLastUpdatedmachine;
    private string branchID;

    public string PointsID { get => strPointsID; set => strPointsID = value; }
    public string? NetPrice { get => strNetPrice; set => strNetPrice = value; }
    public string? NetPoints { get => strNetpoints; set => strNetpoints = value; }
    public string? LastUpdatedDate { get => strLastUpdatedDate; set => strLastUpdatedDate = value; }
    public string? LastUpdatedUser { get => strLastUpdatedUser; set => strLastUpdatedUser = value; }
    public string? LastUpdatedmachine { get => strLastUpdatedmachine; set => strLastUpdatedmachine = value; }
    public string BranchID { get => branchID; set => branchID = value; }
}
