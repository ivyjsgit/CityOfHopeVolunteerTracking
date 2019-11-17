using Microsoft.EntityFrameworkCore.Migrations;

namespace CoHO.Migrations
{
    public partial class lastActivityiID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastActivityID",
                table: "Volunteer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastActivityID",
                table: "Volunteer");
        }
    }
}
