using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleApi.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderGroupIdToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderGroupId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderGroupId",
                table: "Orders");
        }
    }
}
