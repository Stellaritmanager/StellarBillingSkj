using System.ComponentModel.DataAnnotations;

namespace StellarBillingSystem_skj.Models
{
    public class RepledgeProductModel
    {
        public RepledgeProductModel() { }

        public string RepledgeID { get; set; }

        public string BillID { get; set; }

        public string ProductID { get; set; }

        public string IsDelete { get; set; }
        [MaxLength(50)]
        public string? LastUpdatedUser { get; set; }
        [MaxLength(50)]
        public string? LastUpdatedmachine { get; set; }
        [MaxLength(50)]
        public string? LastUpdatedDate { get; set; }
    }
}
