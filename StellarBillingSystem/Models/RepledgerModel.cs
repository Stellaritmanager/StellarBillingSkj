namespace StellarBillingSystem_skj.Models
{
    public class RepledgerModel
    {
        public RepledgerModel() { }

        public int RepledgerID { get; set; }

        public string RepledgerName { get; set; }

        public string RepledgerAddress { get; set; }

        public string Country { get; set; }


        public string State { get; set; }


        public string City { get; set; }

        public string Pincode { get; set; }


        public string RepledgerPhoneNumber1 { get; set; }

        public string? RepledgerPhoneNumber2 { get; set; }

        public string? LastUpdatedDate { get; set; }
        public string? LastUpdatedUser { get; set; }
        public string? LastUpdatedMachine { get; set; }

        public bool IsDelete { get; set; }
    }
}
