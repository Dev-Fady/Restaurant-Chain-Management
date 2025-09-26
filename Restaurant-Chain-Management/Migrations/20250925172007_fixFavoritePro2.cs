using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Chain_Management.Migrations
{
    /// <inheritdoc />
    public partial class fixFavoritePro2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GlobalCode",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
        UPDATE Products 
        SET GlobalCode = CONCAT('PRD-', NEWID())
        WHERE GlobalCode = '' OR GlobalCode IS NULL
    ");

            migrationBuilder.CreateIndex(
                name: "IX_Products_GlobalCode",
                table: "Products",
                column: "GlobalCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_GlobalCode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GlobalCode",
                table: "Products");
        }
    }
}
