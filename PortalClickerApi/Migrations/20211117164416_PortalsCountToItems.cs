using Microsoft.EntityFrameworkCore.Migrations;

namespace PortalClickerApi.Migrations
{
    public partial class PortalsCountToItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Portals",
                table: "SystemItems",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Portals",
                table: "SystemItems");
        }
    }
}
