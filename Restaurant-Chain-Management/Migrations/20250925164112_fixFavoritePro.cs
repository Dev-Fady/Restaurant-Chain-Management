using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Chain_Management.Migrations
{
    /// <inheritdoc />
    public partial class fixFavoritePro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FavoriteProducts_UserId",
                table: "FavoriteProducts");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_UserId_ProductId_stockId",
                table: "FavoriteProducts",
                columns: new[] { "UserId", "ProductId", "stockId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FavoriteProducts_UserId_ProductId_stockId",
                table: "FavoriteProducts");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_UserId",
                table: "FavoriteProducts",
                column: "UserId");
        }
    }
}
