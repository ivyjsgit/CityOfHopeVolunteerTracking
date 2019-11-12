using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CityOfHopeVolunteerTracking.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Initiative",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    First = table.Column<string>(nullable: true),
                    InActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiative", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Volunteer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
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
                    table.PrimaryKey("PK_Volunteer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerActivity",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InitiativeID = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    ElapsedTime = table.Column<float>(nullable: false),
                    ClockedIn = table.Column<bool>(nullable: false),
                    VolunteerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerActivity", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VolunteerActivity_Initiative_InitiativeID",
                        column: x => x.InitiativeID,
                        principalTable: "Initiative",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VolunteerActivity_Volunteer_VolunteerID",
                        column: x => x.VolunteerID,
                        principalTable: "Volunteer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerActivity_InitiativeID",
                table: "VolunteerActivity",
                column: "InitiativeID");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerActivity_VolunteerID",
                table: "VolunteerActivity",
                column: "VolunteerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "VolunteerActivity");

            migrationBuilder.DropTable(
                name: "Initiative");

            migrationBuilder.DropTable(
                name: "Volunteer");
        }
    }
}
