using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electrociti.Migrations
{
    public partial class EndX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentEmployees2_Comments_CommentId",
                table: "CommentEmployees2");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentEmployees2_Employee2_EmployeeId",
                table: "CommentEmployees2");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeService_Employee2_EmployeeId",
                table: "EmployeeService");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeWork_Employee2_EmployeeId",
                table: "EmployeeWork");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases2_Employee2_EmployeeId",
                table: "Purchases2");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Purchases2",
                table: "Purchases2");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee2",
                table: "Employee2");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentEmployees2",
                table: "CommentEmployees2");

            migrationBuilder.RenameTable(
                name: "Purchases2",
                newName: "Purchases");

            migrationBuilder.RenameTable(
                name: "Employee2",
                newName: "Employee");

            migrationBuilder.RenameTable(
                name: "CommentEmployees2",
                newName: "CommentEmployees");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases2_EmployeeId",
                table: "Purchases",
                newName: "IX_Purchases_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentEmployees2_EmployeeId",
                table: "CommentEmployees",
                newName: "IX_CommentEmployees_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentEmployees2_CommentId",
                table: "CommentEmployees",
                newName: "IX_CommentEmployees_CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Purchases",
                table: "Purchases",
                column: "PurchaseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentEmployees",
                table: "CommentEmployees",
                column: "CommentEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEmployees_Comments_CommentId",
                table: "CommentEmployees",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEmployees_Employee_EmployeeId",
                table: "CommentEmployees",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeService_Employee_EmployeeId",
                table: "EmployeeService",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeWork_Employee_EmployeeId",
                table: "EmployeeWork",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Employee_EmployeeId",
                table: "Purchases",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentEmployees_Comments_CommentId",
                table: "CommentEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentEmployees_Employee_EmployeeId",
                table: "CommentEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeService_Employee_EmployeeId",
                table: "EmployeeService");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeWork_Employee_EmployeeId",
                table: "EmployeeWork");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Employee_EmployeeId",
                table: "Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Purchases",
                table: "Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentEmployees",
                table: "CommentEmployees");

            migrationBuilder.RenameTable(
                name: "Purchases",
                newName: "Purchases2");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employee2");

            migrationBuilder.RenameTable(
                name: "CommentEmployees",
                newName: "CommentEmployees2");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_EmployeeId",
                table: "Purchases2",
                newName: "IX_Purchases2_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentEmployees_EmployeeId",
                table: "CommentEmployees2",
                newName: "IX_CommentEmployees2_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentEmployees_CommentId",
                table: "CommentEmployees2",
                newName: "IX_CommentEmployees2_CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Purchases2",
                table: "Purchases2",
                column: "PurchaseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee2",
                table: "Employee2",
                column: "EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentEmployees2",
                table: "CommentEmployees2",
                column: "CommentEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEmployees2_Comments_CommentId",
                table: "CommentEmployees2",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEmployees2_Employee2_EmployeeId",
                table: "CommentEmployees2",
                column: "EmployeeId",
                principalTable: "Employee2",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeService_Employee2_EmployeeId",
                table: "EmployeeService",
                column: "EmployeeId",
                principalTable: "Employee2",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeWork_Employee2_EmployeeId",
                table: "EmployeeWork",
                column: "EmployeeId",
                principalTable: "Employee2",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases2_Employee2_EmployeeId",
                table: "Purchases2",
                column: "EmployeeId",
                principalTable: "Employee2",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
