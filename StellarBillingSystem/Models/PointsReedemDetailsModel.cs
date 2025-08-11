public class PointsReedemDetailsModel
{
    public PointsReedemDetailsModel()
    {
    }


    private String strCustomerID;
    private String strTotalPoints;
    private String strExpiryDate;
    private String strLastUpdatedDate;
    private String strLastUpdatedUser;
    private String strLastUpdatedmachine;
    private String branchID;

    public string CustomerID { get => strCustomerID; set => strCustomerID = value; }
    public string? TotalPoints { get => strTotalPoints; set => strTotalPoints = value; }
    public string? ExpiryDate { get => strExpiryDate; set => strExpiryDate = value; }
    public string? LastUpdatedDate { get => strLastUpdatedDate; set => strLastUpdatedDate = value; }
    public string? LastUpdatedUser { get => strLastUpdatedUser; set => strLastUpdatedUser = value; }
    public string? LastUpdatedmachine { get => strLastUpdatedmachine; set => strLastUpdatedmachine = value; }
    public string BranchID { get => branchID; set => branchID = value; }
}

