namespace StellarBillingSystem_skj.Models
{
    public class RepledgeViewModel
    {
        public string BillID { get; set; }

        public List<ArticleSelection> Articles { get; set; } = new();
        public List<int> SelectedArticleIDs { get; set; } = new();

        public string RepledgeID { get; set; }
        public string? BuyerName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Interest { get; set; }
        public int Tenure { get; set; }

        public bool IsClosed { get; set; }

        public string? CloseDate { get; set; }

        public string? LoanHolderName { get; set; }

        public string? RepledgeDate { get; set; }

        public string? BranchID { get; set; }
    }

    public class ArticleSelection
    {
        public int ArticleID { get; set; }

        public string BillID { get; set; }
        public string? ArticleName { get; set; }
    }

}
