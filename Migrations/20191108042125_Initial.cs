using Microsoft.EntityFrameworkCore.Migrations;

namespace CityOfHopeVolunteerTracking.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Volunteer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    First = table.Column<string>(nullable: true),
                    Last = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Home = table.Column<string>(nullable: true),
                    Cell = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    CommunityService = table.Column<bool>(nullable: false),
                    WorkersComp = table.Column<bool>(nullable: false),
                    Admin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteer", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Volunteer");
        }
    }
}
