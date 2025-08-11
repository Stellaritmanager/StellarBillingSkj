using System.ComponentModel.DataAnnotations;

namespace StellarBillingSystem_skj.Models
{
    public class CustomerImageModel
    {
        public CustomerImageModel() { }

        public int ImageID { get; set; }


        [MaxLength(100)]
        public string CustomerName { get; set; }
        [MaxLength(20)]
        public string MobileNumber { get; set; }
        [MaxLength(100)]
        public string BranchID { get; set; }
        [MaxLength(100)]
        public string ImagePath { get; set; }
        [MaxLength(100)]
        public string FileName { get; set; }
        [MaxLength(50)]
        public DateTime? LastUpdatedDate { get; set; }
        [MaxLength(50)]
        public string? LastUpateUser { get; set; }
        [MaxLength(50)]
        public string? LastUpdatedMachine { get; set; }
        public bool IsPrimary { get; set; }

        public bool IsDelete { get; set; }

    }
}
