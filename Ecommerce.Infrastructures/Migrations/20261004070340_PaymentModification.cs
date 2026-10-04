using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PaymentModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "DeliveryMethods",
                type: "decimal(18,2",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "BasketId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "DeliveryMethods",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2");
        }
    }
}
