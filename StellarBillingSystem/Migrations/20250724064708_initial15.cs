using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StellarBillingSystem_skj.Migrations
{
    /// <inheritdoc />
    public partial class initial15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClosingStatus",
                table: "Shbillmasterskj",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleDate",
                table: "Shbillmasterskj",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalePayment",
                table: "Shbillmasterskj",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleRemark",
                table: "Shbillmasterskj",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "SHArticleMaster",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingStatus",
                table: "Shbillmasterskj");

            migrationBuilder.DropColumn(
                name: "SaleDate",
                table: "Shbillmasterskj");

            migrationBuilder.DropColumn(
                name: "SalePayment",
                table: "Shbillmasterskj");

            migrationBuilder.DropColumn(
                name: "SaleRemark",
                table: "Shbillmasterskj");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "SHArticleMaster");
        }
    }
}
