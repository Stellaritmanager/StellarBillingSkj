using System.ComponentModel.DataAnnotations;

namespace StellarBillingSystem_skj.Models
{
    public class BillMasterModelSKJ
    {
        public BillMasterModelSKJ() { }

        public string BillID { get; set; }

        public string BillDate { get; set; }

        public string CustomerID { get; set; }

        public double OverallWeight { get; set; }

        public decimal TotalValue { get; set; }

        public decimal LoanValue { get; set; }

        public double InitialInterest { get; set; }

        public decimal TotalRepayValue { get; set; }

        public string NoOfItem { get; set; }

        public string? PostTenureInterest { get; set; }

        public string Tenure { get; set; }

        public string? ClosedDate { get; set; }

        public string? closedBy { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        [MaxLength(50)]
        public string? Lastupdateduser { get; set; }
        [MaxLength(50)]
        public string? Lastupdateddate { get; set; }
        [MaxLength(50)]
        public string? Lastupdatedmachine { get; set; }
        public string BranchID { get; set; }

        public string? TotalvalueinWords { get; set; }

        public string? ClosingStatus { get; set; }

        public string? SalePayment { get; set; }
        public string? SaleDate { get; set; }
        public string? SaleRemark { get; set; }



    }
}
