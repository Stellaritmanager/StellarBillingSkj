namespace StellarBillingSystem.Models
{
    public class SignUpModel
    {
        public SignUpModel() { }

        private String strUsername;
        private String strEmail;
        private String strPassword;
        private String strLastUpdatedDate;
        private String strLastUpdatedUser;
        private String strLastUpdatedmachine;

        public string Username { get => strUsername; set => strUsername = value; }
        public string Email { get => strEmail; set => strEmail = value; }
        public string Password { get => strPassword; set => strPassword = value; }
        public string LastUpdatedDate { get => strLastUpdatedDate; set => strLastUpdatedDate = value; }
        public string LastUpdatedUser { get => strLastUpdatedUser; set => strLastUpdatedUser = value; }
        public string LastUpdatedmachine { get => strLastUpdatedmachine; set => strLastUpdatedmachine = value; }
    }
}
