using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Chain_Management.Migrations
{
    /// <inheritdoc />
    public partial class FixCityBranchRelation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Cities_CityId1",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_CityId1",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "CityId1",
                table: "Branches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId1",
                table: "Branches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CityId1",
                table: "Branches",
                column: "CityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Cities_CityId1",
                table: "Branches",
                column: "CityId1",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
