using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StellarBillingSystem_skj.Migrations
{
    /// <inheritdoc />
    public partial class initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SBLogs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LogMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogScreens = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Att1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SBLogs", x => new { x.LogID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHbilldetails",
                columns: table => new
                {
                    ProductID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    BillID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Totalprice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDiscount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Lastupdateduser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastupdateddate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastupdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHbilldetails", x => new { x.Id, x.ProductID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHBillingPoints",
                columns: table => new
                {
                    BillID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NetPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Points = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    DateofReedem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHBillingPoints", x => new { x.BillID, x.CustomerNumber, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHbillmaster",
                columns: table => new
                {
                    BillDate = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    BillID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Totalprice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDiscount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Lastupdateduser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastupdateddate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastupdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CGSTPercentage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SGSTPercentage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CGSTPercentageAmt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SGSTPercentageAmt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Billby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillInsertion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHbillmaster", x => new { x.Id, x.BillDate, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHBranchMaster",
                columns: table => new
                {
                    BracnchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFranchise = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastUpdatedMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchInitial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHBranchMaster", x => new { x.BracnchID, x.BranchName });
                });

            migrationBuilder.CreateTable(
                name: "SHCategoryMaster",
                columns: table => new
                {
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    CategoryID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHCategoryMaster", x => new { x.Id, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHCustomerBilling",
                columns: table => new
                {
                    BillID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Items = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointsNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryBasedDiscount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHCustomerBilling", x => new { x.BillID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHCustomerMaster",
                columns: table => new
                {
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateofBirth = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Fathername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Pincode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHCustomerMaster", x => new { x.MobileNumber, x.CustomerName, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHDiscountCategory",
                columns: table => new
                {
                    CategoryID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHDiscountCategory", x => new { x.CategoryID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "ShGenericReport",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportQuery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Datecolumn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDashboard = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShGenericReport", x => new { x.ReportId, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHGodown",
                columns: table => new
                {
                    ProductID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumberofStocks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatefofPurchase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHGodown", x => new { x.ProductID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHGSTMaster",
                columns: table => new
                {
                    TaxID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SGST = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CGST = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherTax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHGSTMaster", x => new { x.TaxID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHNetDiscountMaster",
                columns: table => new
                {
                    NetID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NetDiscount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHNetDiscountMaster", x => new { x.NetID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHPaymentDetails",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentDiscription = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Lastupdateduser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastupdateddate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastupdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHPaymentDetails", x => new { x.PaymentDiscription, x.PaymentId, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHPaymentMaster",
                columns: table => new
                {
                    BillId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Balance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Lastupdateduser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastupdateddate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastupdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHPaymentMaster", x => new { x.BillId, x.PaymentId, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHPointsMaster",
                columns: table => new
                {
                    PointsID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NetPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetPoints = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHPointsMaster", x => new { x.PointsID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHPointsReedemDetails",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalPoints = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHPointsReedemDetails", x => new { x.CustomerID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHProductMaster",
                columns: table => new
                {
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    ProductID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brandname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    BarcodeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SGST = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CGST = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherTax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImeiNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHProductMaster", x => new { x.Id, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHRackMaster",
                columns: table => new
                {
                    PartitionID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RackID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FacilityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHRackMaster", x => new { x.PartitionID, x.RackID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHRackPartionProduct",
                columns: table => new
                {
                    PartitionID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Noofitems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Isdelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHRackPartionProduct", x => new { x.PartitionID, x.ProductID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHReedemHistory",
                columns: table => new
                {
                    CustomerNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfReedem = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReedemPoints = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Lastupdateduser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastupdateddate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastupdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHReedemHistory", x => new { x.CustomerNumber, x.DateOfReedem, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHReportModel",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportQuery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdateduser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastupdateddate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHReportModel", x => x.ReportId);
                });

            migrationBuilder.CreateTable(
                name: "SHresourceType",
                columns: table => new
                {
                    ResourceTypeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResourceTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastUpdatedMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHresourceType", x => new { x.ResourceTypeID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHRoleaccessModel",
                columns: table => new
                {
                    RollID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScreenID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Access = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Authorized = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastUpdatedMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Isdelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHRoleaccessModel", x => new { x.RollID, x.ScreenID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHrollaccess",
                columns: table => new
                {
                    StaffID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RollID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastupdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastupdatedMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastupdateduser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHrollaccess", x => new { x.StaffID, x.RollID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHrollType",
                columns: table => new
                {
                    RollID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RollName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastupdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastupdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastupdatedMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHrollType", x => new { x.RollID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHScreenMaster",
                columns: table => new
                {
                    ScreenId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScreenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastUpdatedMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHScreenMaster", x => new { x.ScreenId, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHScreenName",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScreenName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHScreenName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SHSignUp",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHSignUp", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "SHStaffAdmin",
                columns: table => new
                {
                    StaffID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResourceTypeID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Initial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdProofId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdProofName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastupdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedMachine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IdProofFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LastupdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RolltypeID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHStaffAdmin", x => new { x.StaffID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHVoucherDetails",
                columns: table => new
                {
                    VoucherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHVoucherDetails", x => new { x.VoucherID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHVoucherMaster",
                columns: table => new
                {
                    VoucherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VocherCost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VocherDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedmachine = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHVoucherMaster", x => new { x.VoucherID, x.BranchID });
                });

            migrationBuilder.CreateTable(
                name: "SHWebErrors",
                columns: table => new
                {
                    ErrodDesc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScreenName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ErrDateTime = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHWebErrors", x => new { x.ErrodDesc, x.ErrDateTime, x.ScreenName });
                });

            migrationBuilder.CreateTable(
                name: "ShcustomerImageMaster",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: true),
                    LastUpateUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastUpdatedMachine = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShcustomerImageMaster", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_ShcustomerImageMaster_SHCustomerMaster_MobileNumber_CustomerName_BranchID",
                        columns: x => new { x.MobileNumber, x.CustomerName, x.BranchID },
                        principalTable: "SHCustomerMaster",
                        principalColumns: new[] { "MobileNumber", "CustomerName", "BranchID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShcustomerImageMaster_MobileNumber_CustomerName_BranchID",
                table: "ShcustomerImageMaster",
                columns: new[] { "MobileNumber", "CustomerName", "BranchID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SBLogs");

            migrationBuilder.DropTable(
                name: "SHbilldetails");

            migrationBuilder.DropTable(
                name: "SHBillingPoints");

            migrationBuilder.DropTable(
                name: "SHbillmaster");

            migrationBuilder.DropTable(
                name: "SHBranchMaster");

            migrationBuilder.DropTable(
                name: "SHCategoryMaster");

            migrationBuilder.DropTable(
                name: "SHCustomerBilling");

            migrationBuilder.DropTable(
                name: "ShcustomerImageMaster");

            migrationBuilder.DropTable(
                name: "SHDiscountCategory");

            migrationBuilder.DropTable(
                name: "ShGenericReport");

            migrationBuilder.DropTable(
                name: "SHGodown");

            migrationBuilder.DropTable(
                name: "SHGSTMaster");

            migrationBuilder.DropTable(
                name: "SHNetDiscountMaster");

            migrationBuilder.DropTable(
                name: "SHPaymentDetails");

            migrationBuilder.DropTable(
                name: "SHPaymentMaster");

            migrationBuilder.DropTable(
                name: "SHPointsMaster");

            migrationBuilder.DropTable(
                name: "SHPointsReedemDetails");

            migrationBuilder.DropTable(
                name: "SHProductMaster");

            migrationBuilder.DropTable(
                name: "SHRackMaster");

            migrationBuilder.DropTable(
                name: "SHRackPartionProduct");

            migrationBuilder.DropTable(
                name: "SHReedemHistory");

            migrationBuilder.DropTable(
                name: "SHReportModel");

            migrationBuilder.DropTable(
                name: "SHresourceType");

            migrationBuilder.DropTable(
                name: "SHRoleaccessModel");

            migrationBuilder.DropTable(
                name: "SHrollaccess");

            migrationBuilder.DropTable(
                name: "SHrollType");

            migrationBuilder.DropTable(
                name: "SHScreenMaster");

            migrationBuilder.DropTable(
                name: "SHScreenName");

            migrationBuilder.DropTable(
                name: "SHSignUp");

            migrationBuilder.DropTable(
                name: "SHStaffAdmin");

            migrationBuilder.DropTable(
                name: "SHVoucherDetails");

            migrationBuilder.DropTable(
                name: "SHVoucherMaster");

            migrationBuilder.DropTable(
                name: "SHWebErrors");

            migrationBuilder.DropTable(
                name: "SHCustomerMaster");
        }
    }
}
