using System.ComponentModel.DataAnnotations;

namespace StellarBillingSystem_skj.Models
{
    public class BuyerDetailsModel
    {
        public BuyerDetailsModel() { }

        public int BuyerID { get; set; }


        [MaxLength(50)]
        public string BuyerName { get; set; }


        [MaxLength(100)]
        public string BuyerAddress { get; set; }

        [MaxLength(20)]
        public string BuyerPhoneNumber { get; set; }

        [MaxLength(50)]
        public string? LastUpdatedUser { get; set; }
        [MaxLength(50)]
        public string? LastUpdatedmachine { get; set; }
        [MaxLength(50)]
        public string? LastUpdatedDate { get; set; }
    }
}
