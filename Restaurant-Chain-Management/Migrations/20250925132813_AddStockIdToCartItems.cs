using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Chain_Management.Migrations
{
    /// <inheritdoc />
    public partial class AddStockIdToCartItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Customers",
                newName: "Address");

            migrationBuilder.AddColumn<int>(
                name: "stockId",
                table: "FavoriteProducts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "stockId",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_stockId",
                table: "FavoriteProducts",
                column: "stockId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_stockId",
                table: "CartItems",
                column: "stockId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Stocks_stockId",
                table: "CartItems",
                column: "stockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProducts_Stocks_stockId",
                table: "FavoriteProducts",
                column: "stockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Stocks_stockId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProducts_Stocks_stockId",
                table: "FavoriteProducts");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteProducts_stockId",
                table: "FavoriteProducts");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_stockId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "stockId",
                table: "FavoriteProducts");

            migrationBuilder.DropColumn(
                name: "stockId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Customers",
                newName: "title");
        }
    }
}
