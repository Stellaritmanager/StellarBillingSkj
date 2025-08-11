namespace StellarBillingSystem.Models
{
    public class StaffAdminModel
    {
        public StaffAdminModel() { }

        private string strStaffID;
        private string strResourceTypeID;
        private string strFirstName;
        private string strLastName;
        private string strFullName;
        private string strInitial;
        private string strPrefix;
        private string strGender;
        private string strDateofBirth;
        private string strAge;
        private string strAddress1;
        private string strCity;
        private string strState;
        private string strPin;
        private string strPhoneNumber;
        private string strEmailId;
        private string strNationality;
        private string strUserName;
        private string strPassword;
        private string strIdProofId;
        private string strIdProofName;
        private string lastupdatedUser;
        private DateTime lastupdatedDate;
        private string lastUpdatedMachine;
        private bool isDelete;
        private string branchID;
        private byte[] idProofFile;
        private string rolltype;



        public string StaffID { get => strStaffID; set => strStaffID = value; }
        public string? ResourceTypeID { get => strResourceTypeID; set => strResourceTypeID = value; }
        public string? FirstName { get => strFirstName; set => strFirstName = value; }
        public string? LastName { get => strLastName; set => strLastName = value; }
        public string? FullName { get => strFullName; set => strFullName = value; }
        public string? Initial { get => strInitial; set => strInitial = value; }
        public string? Prefix { get => strPrefix; set => strPrefix = value; }
        public string? Gender { get => strGender; set => strGender = value; }
        public string? DateofBirth { get => strDateofBirth; set => strDateofBirth = value; }
        public string? Age { get => strAge; set => strAge = value; }
        public string? Address1 { get => strAddress1; set => strAddress1 = value; }
        public string? City { get => strCity; set => strCity = value; }
        public string? State { get => strState; set => strState = value; }
        public string? Pin { get => strPin; set => strPin = value; }
        public string? PhoneNumber { get => strPhoneNumber; set => strPhoneNumber = value; }
        public string? EmailId { get => strEmailId; set => strEmailId = value; }
        public string? Nationality { get => strNationality; set => strNationality = value; }
        public string? UserName { get => strUserName; set => strUserName = value; }
        public string? Password { get => strPassword; set => strPassword = value; }
        public string? IdProofId { get => strIdProofId; set => strIdProofId = value; }
        public string? IdProofName { get => strIdProofName; set => strIdProofName = value; }
        public string? LastupdatedUser { get => lastupdatedUser; set => lastupdatedUser = value; }

        public string? LastUpdatedMachine { get => lastUpdatedMachine; set => lastUpdatedMachine = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }
        public string BranchID { get => branchID; set => branchID = value; }
        public byte[]? IdProofFile { get => idProofFile; set => idProofFile = value; }
        public DateTime LastupdatedDate { get => lastupdatedDate; set => lastupdatedDate = value; }
        public string RolltypeID { get => rolltype; set => rolltype = value; }
    }
}
