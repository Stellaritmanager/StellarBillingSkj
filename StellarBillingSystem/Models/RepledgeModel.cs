using System.ComponentModel.DataAnnotations;

namespace StellarBillingSystem_skj.Models
{
    public class RepledgeModel
    {
        public RepledgeModel() { }

        public int BuyerID { get; set; } // Auto-generated, NOT a primary key

        [Key]
        public string RepledgeID { get; set; } // Primary Key (string, manually set)

        public string BuyerName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal Interest { get; set; }
        public int Tenure { get; set; }

        public string BranchID { get; set; }

        public bool IsDelete { get; set; }
        public string? LastUpdatedDate { get; set; }
        public string? LastUpdatedUser { get; set; }
        public string? LastUpdatedMachine { get; set; }

        public bool IsClosed {  get; set; }

        public string? CloseDate {  get; set; }

        public string? RepledgeDate { get; set;}

        public string? LoanHolderName { get; set; }
    }
}
