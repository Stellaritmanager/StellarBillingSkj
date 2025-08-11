using System.ComponentModel.DataAnnotations;

namespace StellarBillingSystem_skj.Models
{
    public class BillDetailsModelSKJ
    {
        public BillDetailsModelSKJ() { }

        public string BillID { get; set; }

        public bool IsDelete { get; set; }
        [MaxLength(50)]
        public string? Lastupdateduser { get; set; }
        [MaxLength(50)]
        public string? Lastupdateddate { get; set; }
        [MaxLength(50)]
        public string? Lastupdatedmachine { get; set; }
        public string BranchID { get; set; }
        public double Grossweight { get; set; }
        public double Netweight { get; set; }
        public double Reducedweight { get; set; }
        public decimal Netmarketprice { get; set; }
        public decimal Apprisevaluepergram { get; set; }
        public decimal Apprisenetvalue { get; set; }

        public int ArticleID { get; set; }

        public int Quantity { get; set; }
    }
}
