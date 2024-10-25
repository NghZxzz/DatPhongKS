using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnDatPhongKS.Migrations
{
    /// <inheritdoc />
    public partial class chinhsua : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "QuatityCustomer",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuatityCustomer",
                table: "Products");
        }
    }
}
