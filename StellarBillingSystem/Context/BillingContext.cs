
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using StellarBillingSystem.Models;
using StellarBillingSystem_skj.Models;
namespace StellarBillingSystem.Context
{
    public class BillingContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BillingContext() { }

        public BillingContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //LogTable
        public DbSet<LogsModel> SBLogs { get; set; }
        public DbSet<ProductMatserModel> SHProductMaster { get; set; }

        public DbSet<BilingSysytemModel> SHCustomerBilling { get; set; }

        public DbSet<DiscountCategoryMasterModel> SHDiscountCategory { get; set; }

        public DbSet<GSTMasterModel> SHGSTMaster { get; set; }

        public DbSet<VoucherCustomerDetailModel> SHVoucherDetails { get; set; }

        public DbSet<GodownModel> SHGodown { get; set; }

        public DbSet<NetDiscountMasterModel> SHNetDiscountMaster { get; set; }

        public DbSet<VoucherMasterModel> SHVoucherMaster { get; set; }

        public DbSet<RackPatrionProductModel> SHRackPartionProduct { get; set; }

        public DbSet<RackMasterModel> SHRackMaster { get; set; }

        public DbSet<PointsReedemDetailsModel> SHPointsReedemDetails { get; set; }

        public DbSet<PointsMasterModel> SHPointsMaster { get; set; }

        public DbSet<ReportModel> SHReportModel { get; set; }

        public DbSet<SignUpModel> SHSignUp { get; set; }

        public DbSet<StaffAdminModel> SHStaffAdmin { get; set; }

        public DbSet<ResourceTypeMasterModel> SHresourceType { get; set; }

        public DbSet<RollAccessMaster> SHrollaccess { get; set; }

        public DbSet<RoleAccessModel> SHRoleaccessModel { get; set; }

        public DbSet<RollTypeMaster> SHrollType { get; set; }

        public DbSet<ScreenMasterModel> SHScreenMaster { get; set; }

        public DbSet<ScreenNameMasterModel> SHScreenName { get; set; }

        public DbSet<GenericReportModel> ShGenericReport { get; set; }

        public DbSet<BillingMasterModel> SHbillmaster { get; set; }

        public DbSet<BillingDetailsModel> SHbilldetails { get; set; }

        public DbSet<PaymentMasterModel> SHPaymentMaster { get; set; }

        public DbSet<PaymentDetailsModel> SHPaymentDetails { get; set; }

        public DbSet<ReedemHistoryModel> SHReedemHistory { get; set; }

        public DbSet<BranchMasterModel> SHBranchMaster { get; set; }

        public DbSet<WebErrorsModel> SHWebErrors { get; set; }

        public DbSet<BillingPointsModel> SHBillingPoints { get; set; }



        //Table creation for skj

        public DbSet<CustomerMasterModel> SHCustomerMaster { get; set; }

        public DbSet<CustomerImageModel> ShcustomerImageMaster { get; set; }

        public DbSet<CategoryMasterModel> SHCategoryMaster { get; set; }

        public DbSet<ArticleModel> SHArticleMaster { get; set; }

        public DbSet<BillDetailsModelSKJ> Shbilldetailsskj { get; set; }

        public DbSet<BillMasterModelSKJ> Shbillmasterskj { get; set; }

        public DbSet<BillImageModelSKJ> Shbillimagemodelskj { get; set; }

        public DbSet<BillIDCombinationModel> Shbillcombinationskj { get; set; }

        public DbSet<RepledgeModel> Shbuyerrepledge { get; set; }

        public DbSet<RepledgeArtcileModel> Shrepledgeartcile { get; set; }

        public DbSet<RepledgerModel> Shrepledgermodel { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=StellarBilling;Integrated Security=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<WebErrorsModel>()
        .HasKey(i => new { i.ErrodDesc, i.ErrDateTime, i.ScreenName });

            modelBuilder.Entity<BillingPointsModel>().HasKey(i => new { i.BillID, i.CustomerNumber, i.BranchID });
            modelBuilder.Entity<BranchMasterModel>().HasKey(i => new { i.BracnchID, i.BranchName });

            modelBuilder.Entity<BillingMasterModel>().Property(i => i.Id).UseIdentityColumn(101, 1);
            modelBuilder.Entity<BillingMasterModel>().HasKey(i => new { i.Id, i.BillDate, i.BranchID });

            modelBuilder.Entity<BillingDetailsModel>().Property(i => i.Id).UseIdentityColumn(101, 1);
            modelBuilder.Entity<BillingDetailsModel>().HasKey(i => new { i.Id, i.ProductID, i.BranchID });

            modelBuilder.Entity<GenericReportModel>().HasKey(i => new { i.ReportId, i.BranchID });

            modelBuilder.Entity<StaffAdminModel>().HasKey(i => new { i.StaffID, i.BranchID });
            modelBuilder.Entity<ResourceTypeMasterModel>().HasKey(i => new { i.ResourceTypeID, i.BranchID });
            modelBuilder.Entity<RollAccessMaster>().HasKey(i => new { i.StaffID, i.RollID, i.BranchID });
            modelBuilder.Entity<RoleAccessModel>().HasKey(i => new { i.RollID, i.ScreenID, i.BranchID });
            modelBuilder.Entity<RollTypeMaster>().HasKey(i => new { i.RollID, i.BranchID });
            modelBuilder.Entity<ScreenMasterModel>().HasKey(i => new { i.ScreenId, i.BranchID });
            modelBuilder.Entity<ScreenNameMasterModel>().HasKey(i => new { i.Id });



            modelBuilder.Entity<LogsModel>().HasKey(i => new { i.LogID, i.BranchID });



            modelBuilder.Entity<ProductMatserModel>().Property(i => i.Id).UseIdentityColumn(101, 1);
            modelBuilder.Entity<ProductMatserModel>().HasKey(i => new { i.Id, i.BranchID });

            modelBuilder.Entity<DiscountCategoryMasterModel>().HasKey(i => new { i.CategoryID, i.BranchID });

            modelBuilder.Entity<GSTMasterModel>().HasKey(i => new { i.TaxID, i.BranchID });

            modelBuilder.Entity<VoucherCustomerDetailModel>().HasKey(i => new { i.VoucherID, i.BranchID });

            modelBuilder.Entity<BilingSysytemModel>().HasKey(i => new { i.BillID, i.BranchID });

            modelBuilder.Entity<GodownModel>().HasKey(i => new { i.ProductID, i.BranchID });

            modelBuilder.Entity<NetDiscountMasterModel>().HasKey(i => new { i.NetID, i.BranchID });

            modelBuilder.Entity<VoucherMasterModel>().HasKey(i => new { i.VoucherID, i.BranchID });

            modelBuilder.Entity<RackPatrionProductModel>().HasKey(i => new { i.PartitionID, i.ProductID, i.BranchID });

            modelBuilder.Entity<RackMasterModel>().HasKey(i => new { i.PartitionID, i.RackID, i.BranchID });

            modelBuilder.Entity<PointsReedemDetailsModel>().HasKey(i => new { i.CustomerID, i.BranchID });

            modelBuilder.Entity<PointsMasterModel>().HasKey(i => new { i.PointsID, i.BranchID });

            modelBuilder.Entity<ReportModel>().HasKey(i => new { i.ReportId });

            modelBuilder.Entity<SignUpModel>().HasKey(i => new { i.Username });

            modelBuilder.Entity<PaymentMasterModel>().HasKey(i => new { i.BillId, i.PaymentId, i.BranchID });

            modelBuilder.Entity<PaymentDetailsModel>().HasKey(i => new { i.PaymentDiscription, i.PaymentId, i.BranchID });

            modelBuilder.Entity<ReedemHistoryModel>().HasKey(i => new { i.CustomerNumber, i.DateOfReedem, i.BranchID });










            //Model for SKJ

            modelBuilder.Entity<CustomerMasterModel>()
                    .HasKey(c => new { c.MobileNumber, c.CustomerName, c.BranchID });

            modelBuilder.Entity<CustomerImageModel>()
                .HasKey(i => i.ImageID); // Identity PK

            modelBuilder.Entity<CustomerImageModel>()
                .HasIndex(i => new { i.MobileNumber, i.CustomerName, i.BranchID }); // For FK index

            modelBuilder.Entity<CustomerImageModel>()
                .HasOne<CustomerMasterModel>()
                .WithMany() // no navigation
                .HasForeignKey(i => new { i.MobileNumber, i.CustomerName, i.BranchID })
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<CategoryMasterModel>().Property(i => i.Id).UseIdentityColumn(101, 1);

            modelBuilder.Entity<CategoryMasterModel>().HasKey(i => new { i.Id, i.BranchID, i.CategoryName });

            modelBuilder.Entity<ArticleModel>()
                   .HasKey(c => new { c.ArticleID });



            modelBuilder.Entity<BillMasterModelSKJ>()
               .HasKey(c => new { c.BillID, c.BranchID });


            modelBuilder.Entity<BillDetailsModelSKJ>()
                   .HasKey(c => new { c.ArticleID, c.BranchID, c.BillID });

            modelBuilder.Entity<BillDetailsModelSKJ>()
                    .HasOne<ArticleModel>()                     // Point to principal type
                    .WithMany()                                 // No navigation on principal either
                    .HasForeignKey(d => d.ArticleID)            // FK in dependent
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BillDetailsModelSKJ>()
                    .HasOne<BillMasterModelSKJ>()
                    .WithMany() // or .WithMany(m => m.BillDetails)
                    .HasForeignKey(c => new { c.BillID, c.BranchID })
                    .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BillImageModelSKJ>()
               .HasKey(i => i.ImageID); // Identity PK

            modelBuilder.Entity<BillIDCombinationModel>().HasKey(i => i.BranchID);

            // BuyerModelSKJ
            modelBuilder.Entity<RepledgeModel>()
                .HasKey(x => x.RepledgeID);

            modelBuilder.Entity<RepledgeModel>()
                .Property(x => x.BuyerID)
                .ValueGeneratedOnAdd();

            // ReplArticleModel: Composite Key
            modelBuilder.Entity<RepledgeArtcileModel>()
                .HasKey(x => new { x.BillID, x.ArticleID, x.RepledgeID });

            // ReplArticleID: just auto-generated column
            modelBuilder.Entity<RepledgeArtcileModel>()
                .Property(x => x.RepledgeArticleIDS)
                .ValueGeneratedOnAdd();

            // FK to ArticleModel
            modelBuilder.Entity<RepledgeArtcileModel>()
                .HasOne<ArticleModel>()
                .WithMany()
                .HasForeignKey(x => x.ArticleID)
                .OnDelete(DeleteBehavior.Restrict);

            // FK to BillMasterModelSKJ
            modelBuilder.Entity<RepledgeArtcileModel>()
                .HasOne<BillMasterModelSKJ>()
                .WithMany()
                .HasForeignKey(x => new { x.BillID, x.BranchID })
                .OnDelete(DeleteBehavior.Cascade);

            // FK to BuyerModelSKJ
            modelBuilder.Entity<RepledgeArtcileModel>()
                .HasOne<RepledgeModel>()
                .WithMany()
                .HasForeignKey(x => x.RepledgeID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RepledgerModel>()
                .HasKey(i => i.RepledgerID);

        }

        // Override SaveChangesAsync to auto-generate the Ids for various screens with the prefix
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var branchId = _httpContextAccessor.HttpContext?.Session.GetString("BranchID");
            var BillId = _httpContextAccessor.HttpContext?.Session.GetString("BillID");


            //Category Master
            var catMas = ChangeTracker
                        .Entries<CategoryMasterModel>()
                        .Where(e => e.State == EntityState.Added)
                        .ToList();

            if (catMas.Any())
            {
                // Get the latest BillNumber from the database
                var lastCat = await this.SHCategoryMaster.Where(x => x.BranchID == branchId).OrderByDescending(b => b.Id).FirstOrDefaultAsync();
                int lastNumber = 100; // Starting point, e.g., Bill_100

                if (lastCat != null)
                {
                    // Extract the numeric part of the last BillNumber and increment it
                    string lastBillNumber = lastCat.CategoryID.Replace("Gold_", "");
                    if (int.TryParse(lastBillNumber, out int number))
                    {
                        lastNumber = number;
                    }
                }

                // Assign the new BillNumber for each new bill
                foreach (var billEntry in catMas)
                {
                    lastNumber++;
                    billEntry.Entity.CategoryID = $"Gold_{lastNumber}";
                }
            }



            //Product Master
            var ProdMas = ChangeTracker
                        .Entries<ProductMatserModel>()
                        .Where(e => e.State == EntityState.Added)
                        .ToList();

            if (ProdMas.Any())
            {
                // Get the latest BillNumber from the database
                var lastProd = await this.SHProductMaster.Where(x => x.BranchID == branchId).OrderByDescending(b => b.Id).FirstOrDefaultAsync();
                int lastProdNumber = 100; // Starting point, e.g., Bill_100

                if (lastProd != null)
                {
                    // Extract the numeric part of the last BillNumber and increment it
                    string lastProdNum = lastProd.ProductID.Replace("Pro_", "");
                    if (int.TryParse(lastProdNum, out int number))
                    {
                        lastProdNumber = number;
                    }
                }

                // Assign the new BillNumber for each new bill
                foreach (var billEntry in ProdMas)
                {
                    lastProdNumber++;
                    billEntry.Entity.ProductID = $"Pro_{lastProdNumber}";
                }
            }


            //Bill Master
            var BillDet = ChangeTracker
                        .Entries<BillingDetailsModel>()
                        .Where(e => e.State == EntityState.Added)
                        .ToList();


            if (BillDet.Any() && (BillId == null || BillId == string.Empty))
            {
                // Get the latest BillNumber from the database
                var lastBill = await this.SHbillmaster.Where(x => x.BranchID == branchId).OrderByDescending(b => b.Id).FirstOrDefaultAsync();
                int lastBillNumber = 101; // Starting point, e.g., Bill_100

                if (lastBill != null)
                {
                    // Extract the numeric part of the last BillNumber and increment it
                    string lastBillNum = lastBill.BillID.Replace("Bill_", "");
                    if (int.TryParse(lastBillNum, out int number))
                    {
                        lastBillNumber = number;
                    }
                }

                lastBillNumber++;
                // Assign the new BillNumber for each new bill
                foreach (var billEntry in BillDet)
                {
                    billEntry.Entity.BillID = $"Bill_{lastBillNumber}";
                }
            }
            else
            {
                foreach (var billEntry in BillDet)
                {
                    billEntry.Entity.BillID = BillId;
                }
            }

            //Bill Master
            var BillMas = ChangeTracker
                        .Entries<BillingMasterModel>()
                        .Where(e => e.State == EntityState.Added)
                        .ToList();


            if (BillMas.Any())
            {
                // Get the latest BillNumber from the database
                var lastmasBill = await this.SHbillmaster.Where(x => x.BranchID == branchId).OrderByDescending(b => b.Id).FirstOrDefaultAsync();
                int lastmasBillNumber = 101; // Starting point, e.g., Bill_100

                if (lastmasBill != null)
                {
                    // Extract the numeric part of the last BillNumber and increment it
                    string lastBillNum = lastmasBill.BillID.Replace("Bill_", "");
                    if (int.TryParse(lastBillNum, out int number))
                    {
                        lastmasBillNumber = number;
                    }
                }

                // Assign the new BillNumber for each new bill
                foreach (var billEntry in BillMas)
                {
                    lastmasBillNumber++;
                    billEntry.Entity.BillID = $"Bill_{lastmasBillNumber}";
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
