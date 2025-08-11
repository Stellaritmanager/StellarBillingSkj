using System.ComponentModel.DataAnnotations;

namespace StellarBillingSystem_skj.Models
{
    public class BillIDCombinationModel
    {
        public BillIDCombinationModel() { }

        public string CombinationValue { get; set; }
        public string IncrementValue { get; set; }
        public string BranchID { get; set; }


        [MaxLength(50)]
        public string? Lastupdateduser { get; set; }
        [MaxLength(50)]
        public string? Lastupdateddate { get; set; }
        [MaxLength(50)]
        public string? Lastupdatedmachine { get; set; }
    }
}
