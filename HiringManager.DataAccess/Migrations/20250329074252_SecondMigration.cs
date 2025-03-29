using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HiringManager.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationDetails",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ApplicationDetails",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ApplicationDetails",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "StageID",
                table: "ApplicationDetails",
                newName: "StageId");

            migrationBuilder.RenameColumn(
                name: "SessionID",
                table: "ApplicationDetails",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "ApplicationID",
                table: "ApplicationDetails",
                newName: "ApplicationId");

            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "ApplicationDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationId",
                table: "ApplicationDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Submitted",
                table: "ApplicationDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EducationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationDetailId = table.Column<int>(type: "int", nullable: false),
                    HighestQualification = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CollegeUniversity = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Stream = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PassoutYear = table.Column<int>(type: "int", nullable: false),
                    PointerPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationDetails_ApplicationDetails_ApplicationDetailId",
                        column: x => x.ApplicationDetailId,
                        principalTable: "ApplicationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationDetailId = table.Column<int>(type: "int", nullable: false),
                    TotalExperience = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousCompanies = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CurrentCompany = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Roles = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienceDetails_ApplicationDetails_ApplicationDetailId",
                        column: x => x.ApplicationDetailId,
                        principalTable: "ApplicationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationDetailId = table.Column<int>(type: "int", nullable: false),
                    ApplicationDetailId1 = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalDetails_ApplicationDetails_ApplicationDetailId",
                        column: x => x.ApplicationDetailId,
                        principalTable: "ApplicationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalDetails_ApplicationDetails_ApplicationDetailId1",
                        column: x => x.ApplicationDetailId1,
                        principalTable: "ApplicationDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDetails_ApplicationId",
                table: "ApplicationDetails",
                column: "ApplicationId",
                unique: true,
                filter: "[ApplicationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EducationDetails_ApplicationDetailId",
                table: "EducationDetails",
                column: "ApplicationDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceDetails_ApplicationDetailId",
                table: "ExperienceDetails",
                column: "ApplicationDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetails_ApplicationDetailId",
                table: "PersonalDetails",
                column: "ApplicationDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetails_ApplicationDetailId1",
                table: "PersonalDetails",
                column: "ApplicationDetailId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationDetails");

            migrationBuilder.DropTable(
                name: "ExperienceDetails");

            migrationBuilder.DropTable(
                name: "PersonalDetails");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationDetails_ApplicationId",
                table: "ApplicationDetails");

            migrationBuilder.DropColumn(
                name: "Submitted",
                table: "ApplicationDetails");

            migrationBuilder.RenameColumn(
                name: "StageId",
                table: "ApplicationDetails",
                newName: "StageID");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "ApplicationDetails",
                newName: "SessionID");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "ApplicationDetails",
                newName: "ApplicationID");

            migrationBuilder.AlterColumn<int>(
                name: "SessionID",
                table: "ApplicationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationID",
                table: "ApplicationDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "ApplicationDetails",
                columns: new[] { "Id", "ActiveStatus", "ApplicationID", "CreatedBy", "CreatedDate", "SessionID", "StageID", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, true, "20250329-0001", "Yash", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, null, null },
                    { 2, true, "20250329-0002", "Parth", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, null, null },
                    { 3, false, "20250329-0003", "Sangam", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, null, null }
                });
        }
    }
}
