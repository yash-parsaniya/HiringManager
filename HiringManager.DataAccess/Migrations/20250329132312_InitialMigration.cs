using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HiringManager.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActiveStatus = table.Column<bool>(type: "bit", nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationDetailId = table.Column<int>(type: "int", nullable: false),
                    HighestQualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollegeUniversity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stream = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassoutYear = table.Column<int>(type: "int", nullable: false),
                    PointerPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationSet_ApplicationSet_ApplicationDetailId",
                        column: x => x.ApplicationDetailId,
                        principalTable: "ApplicationSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationDetailId = table.Column<int>(type: "int", nullable: false),
                    TotalExperience = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousCompanies = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienceSet_ApplicationSet_ApplicationDetailId",
                        column: x => x.ApplicationDetailId,
                        principalTable: "ApplicationSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationDetailId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalSet_ApplicationSet_ApplicationDetailId",
                        column: x => x.ApplicationDetailId,
                        principalTable: "ApplicationSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationSet_ApplicationDetailId",
                table: "EducationSet",
                column: "ApplicationDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceSet_ApplicationDetailId",
                table: "ExperienceSet",
                column: "ApplicationDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalSet_ApplicationDetailId",
                table: "PersonalSet",
                column: "ApplicationDetailId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationSet");

            migrationBuilder.DropTable(
                name: "ExperienceSet");

            migrationBuilder.DropTable(
                name: "PersonalSet");

            migrationBuilder.DropTable(
                name: "ApplicationSet");
        }
    }
}
