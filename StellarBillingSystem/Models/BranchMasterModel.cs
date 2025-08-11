public class BranchMasterModel
{
    public BranchMasterModel() { }

    private string bracnchID;
    private string branchName;
    private string phoneNumber1;
    private string phoneNumber2;
    private string strAddress1;
    private string strAddress2;
    private string country;
    private string state;
    private string city;
    private string zipCode;
    private string isFranchise;
    private bool isDelete;
    private string stremail;
    private string branchInitial;
    private string strlastUpdatedUser;
    private string strlastUpdatedMachine;
    private string strLastUpdatedDate;
    private string billTemplate;

    public string BracnchID { get => bracnchID; set => bracnchID = value; }
    public string BranchName { get => branchName; set => branchName = value; }
    public string? PhoneNumber1 { get => phoneNumber1; set => phoneNumber1 = value; }
    public string? PhoneNumber2 { get => phoneNumber2; set => phoneNumber2 = value; }
    public string? Address1 { get => strAddress1; set => strAddress1 = value; }
    public string? Address2 { get => strAddress2; set => strAddress2 = value; }
    public string? Country { get => country; set => country = value; }
    public string? State { get => state; set => state = value; }
    public string? City { get => city; set => city = value; }
    public string? ZipCode { get => zipCode; set => zipCode = value; }
    public string? IsFranchise { get => isFranchise; set => isFranchise = value; }
    public bool IsDelete { get => isDelete; set => isDelete = value; }
    public string? email { get => stremail; set => stremail = value; }
    public string? lastUpdatedUser { get => strlastUpdatedUser; set => strlastUpdatedUser = value; }
    public string? lastUpdatedMachine { get => strlastUpdatedMachine; set => strlastUpdatedMachine = value; }
    public string? LastUpdatedDate { get => strLastUpdatedDate; set => strLastUpdatedDate = value; }
    public string? BranchInitial { get => branchInitial; set => branchInitial = value; }
    public string? BillTemplate { get => billTemplate; set => billTemplate = value; }
}

