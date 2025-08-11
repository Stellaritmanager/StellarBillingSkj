public class GSTMasterModel
{
    public GSTMasterModel()
    {
    }
    private String strTaxID;
    private String strSGST;
    private String strCGST;
    private String strOtherTax;
    private String strLastUpdatedDate;
    private String strLastUpdatedUser;
    private String strLastUpdatedmachine;
    private string branchID;

    public string TaxID { get => strTaxID; set => strTaxID = value; }

    public string? SGST { get => strSGST; set => strSGST = value; }
    public string? CGST { get => strCGST; set => strCGST = value; }
    public string? OtherTax { get => strOtherTax; set => strOtherTax = value; }
    public string? LastUpdatedDate { get => strLastUpdatedDate; set => strLastUpdatedDate = value; }
    public string? LastUpdatedUser { get => strLastUpdatedUser; set => strLastUpdatedUser = value; }
    public string? LastUpdatedmachine { get => strLastUpdatedmachine; set => strLastUpdatedmachine = value; }
    public string BranchID { get => branchID; set => branchID = value; }
}
