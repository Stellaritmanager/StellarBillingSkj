using System.ComponentModel.DataAnnotations;

namespace StellarBillingSystem_skj.Models
{
    public class BillImageModelSKJ
    {
        public BillImageModelSKJ() { }

        public string BillID { get; set; }

        public int ImageID { get; set; }

        public string ImagePath { get; set; }

        public string ImageName { get; set; }


        [MaxLength(50)]
        public string? Lastupdateduser { get; set; }
        [MaxLength(50)]
        public string? Lastupdateddate { get; set; }
        [MaxLength(50)]
        public string? Lastupdatedmachine { get; set; }
    }
}
