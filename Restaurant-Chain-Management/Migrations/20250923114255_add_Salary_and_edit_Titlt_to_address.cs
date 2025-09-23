using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Chain_Management.Migrations
{
    /// <inheritdoc />
    public partial class add_Salary_and_edit_Titlt_to_address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Employees",
                newName: "Address");

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "Employees",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Employees",
                newName: "Title");
        }
    }
}
