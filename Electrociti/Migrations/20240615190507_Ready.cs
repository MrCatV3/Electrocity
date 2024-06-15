using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electrociti.Migrations
{
    public partial class Ready : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeWorkStatus",
                table: "EmployeeWork",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeWorkStatus",
                table: "EmployeeWork");
        }
    }
}
