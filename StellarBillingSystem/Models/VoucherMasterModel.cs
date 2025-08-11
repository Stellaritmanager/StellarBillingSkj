public class VoucherMasterModel
{

    public VoucherMasterModel()
    {
    }

    private String strVoucherID;
    private String strVoucherNumber;
    private String strVocherCost;
    private String strVocherDetails;
    private String strExpiryDate;
    private String strLastUpdatedDate;
    private String strLastUpdatedUser;
    private String strLastUpdatedmachine;
    private string branchID;

    public string VoucherID { get => strVoucherID; set => strVoucherID = value; }
    public string? VoucherNumber { get => strVoucherNumber; set => strVoucherNumber = value; }
    public string? VocherCost { get => strVocherCost; set => strVocherCost = value; }
    public string? VocherDetails { get => strVocherDetails; set => strVocherDetails = value; }
    public string? ExpiryDate { get => strExpiryDate; set => strExpiryDate = value; }
    public string? LastUpdatedDate { get => strLastUpdatedDate; set => strLastUpdatedDate = value; }
    public string? LastUpdatedUser { get => strLastUpdatedUser; set => strLastUpdatedUser = value; }
    public string? LastUpdatedmachine { get => strLastUpdatedmachine; set => strLastUpdatedmachine = value; }
    public string BranchID { get => branchID; set => branchID = value; }
}
