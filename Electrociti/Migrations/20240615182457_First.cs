using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electrociti.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                });

            migrationBuilder.CreateTable(
                name: "Employee2",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeePatronomic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeePhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeBirthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeRegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeePassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeRole = table.Column<int>(type: "int", nullable: false),
                    EmployeeDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee2", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceCost = table.Column<int>(type: "int", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "CommentEmployees2",
                columns: table => new
                {
                    CommentEmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentEmployee_ = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    EmployeeComment = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentEmployees2", x => x.CommentEmployeeId);
                    table.ForeignKey(
                        name: "FK_CommentEmployees2_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentEmployees2_Employee2_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee2",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeWork",
                columns: table => new
                {
                    EmployeeWorkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeWorkDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeWorkTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeWorkAdress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeWorkPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeWorkName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWork", x => x.EmployeeWorkId);
                    table.ForeignKey(
                        name: "FK_EmployeeWork_Employee2_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee2",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchases2",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseTotalCost = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchaseEmployee = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases2", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK_Purchases2_Employee2_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee2",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeService",
                columns: table => new
                {
                    EmployeeServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeService", x => x.EmployeeServiceId);
                    table.ForeignKey(
                        name: "FK_EmployeeService_Employee2_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee2",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentEmployees2_CommentId",
                table: "CommentEmployees2",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEmployees2_EmployeeId",
                table: "CommentEmployees2",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeService_EmployeeId",
                table: "EmployeeService",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeService_ServiceId",
                table: "EmployeeService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWork_EmployeeId",
                table: "EmployeeWork",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases2_EmployeeId",
                table: "Purchases2",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentEmployees2");

            migrationBuilder.DropTable(
                name: "EmployeeService");

            migrationBuilder.DropTable(
                name: "EmployeeWork");

            migrationBuilder.DropTable(
                name: "Purchases2");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Employee2");
        }
    }
}
