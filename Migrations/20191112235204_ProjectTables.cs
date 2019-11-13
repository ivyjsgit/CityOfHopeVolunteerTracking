using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoHO.Migrations
{
    public partial class ProjectTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Initiative",
                columns: table => new
                {
                    InitiativeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    First = table.Column<string>(nullable: true),
                    InActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative", x => x.InitiativeID);
                });

            migrationBuilder.CreateTable(
                name: "ValueOfHour",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    Value = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueOfHour", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Volunteer",
                columns: table => new
                {
                    VolunteerID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    First = table.Column<string>(nullable: false),
                    Last = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Home = table.Column<string>(nullable: true),
                    Cell = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    InActive = table.Column<bool>(nullable: false),
                    CommunityService = table.Column<bool>(nullable: false),
                    WorkersComp = table.Column<bool>(nullable: false),
                    Admin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteer", x => x.VolunteerID);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerActivity",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VolunteerId = table.Column<int>(nullable: false),
                    IniativeId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    ElapsedTime = table.Column<float>(nullable: false),
                    ClockedIn = table.Column<bool>(nullable: false),
                    InitiativeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerActivity", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VolunteerActivity_Initiative_InitiativeID",
                        column: x => x.InitiativeID,
                        principalTable: "Initiative",
                        principalColumn: "InitiativeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VolunteerActivity_Volunteer_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteer",
                        principalColumn: "VolunteerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerActivity_InitiativeID",
                table: "VolunteerActivity",
                column: "InitiativeID");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerActivity_VolunteerId",
                table: "VolunteerActivity",
                column: "VolunteerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValueOfHour");

            migrationBuilder.DropTable(
                name: "VolunteerActivity");

            migrationBuilder.DropTable(
                name: "Initiative");

            migrationBuilder.DropTable(
                name: "Volunteer");
        }
    }
}
