using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedOrderLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Bags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IdUser",
                table: "Orders",
                column: "IdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_IdUser",
                table: "Orders",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_IdUser",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_IdUser",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Bags");
        }
    }
}
