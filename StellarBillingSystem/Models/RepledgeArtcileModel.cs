namespace StellarBillingSystem_skj.Models
{
    public class RepledgeArtcileModel
    {
        public RepledgeArtcileModel() { }


        public int RepledgeArticleIDS { get; set; } // Auto-generated, not part of key
        public int ArticleID { get; set; }  // PK, FK to ArticleModel
        public string BillID { get; set; }     // PK, FK to BillMasterModelSKJ
        public string RepledgeID { get; set; }     // PK, FK to BuyerModelSKJ
        public string BranchID { get; set; }

        public string? LastUpdatedDate { get; set; }
        public string? LastUpdatedUser { get; set; }
        public string? LastUpdatedMachine { get; set; }

        public bool IsDelete { get; set; }


    }
}
