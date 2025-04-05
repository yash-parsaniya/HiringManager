using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HiringManager.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActiveStatus = table.Column<bool>(type: "bit", nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HighestQualification = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CollegeUniversity = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Stream = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PassoutYear = table.Column<int>(type: "int", nullable: false),
                    PointerPercentage = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApplicationDetailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationDetails_ApplicationDetails_ApplicationDetailsId",
                        column: x => x.ApplicationDetailsId,
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
                    TotalExp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PreviousCompanies = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentCompany = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Roles = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ApplicationDetailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienceDetails_ApplicationDetails_ApplicationDetailsId",
                        column: x => x.ApplicationDetailsId,
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
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ApplicationDetailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalDetails_ApplicationDetails_ApplicationDetailsId",
                        column: x => x.ApplicationDetailsId,
                        principalTable: "ApplicationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationDetails_ApplicationDetailsId",
                table: "EducationDetails",
                column: "ApplicationDetailsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceDetails_ApplicationDetailsId",
                table: "ExperienceDetails",
                column: "ApplicationDetailsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDetails_ApplicationDetailsId",
                table: "PersonalDetails",
                column: "ApplicationDetailsId",
                unique: true);
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

            migrationBuilder.DropTable(
                name: "ApplicationDetails");
        }
    }
}
