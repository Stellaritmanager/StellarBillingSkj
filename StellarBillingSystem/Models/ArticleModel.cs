using System.ComponentModel.DataAnnotations;

namespace StellarBillingSystem_skj.Models
{
    public class ArticleModel
    {
        public ArticleModel() { }

        public int ArticleID { get; set; }
        [MaxLength(100)]
        public string? ArticleName { get; set; }

        public double WeightOfArticle { get; set; }

        public string GoldType { get; set; }
        [MaxLength(50)]
        public string? LastUpdatedUser { get; set; }
        [MaxLength(50)]
        public string? LastUpdatedmachine { get; set; }
        [MaxLength(50)]
        public string? LastUpdatedDate { get; set; }

        public string BranchID { get; set; }

        public bool IsDelete { get; set; }

        public string? Remarks { get; set; }
    }
}
