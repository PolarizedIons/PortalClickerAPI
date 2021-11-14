using Microsoft.EntityFrameworkCore.Migrations;

namespace PortalClickerApi.Migrations
{
    public partial class AddMultiplierInfoToPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "BaseClickAmount",
                table: "ClickerPlayers",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 1ul);

            migrationBuilder.AddColumn<double>(
                name: "ClickMultiplier",
                table: "ClickerPlayers",
                type: "double",
                nullable: false,
                defaultValue: 1.0);

            migrationBuilder.AddColumn<double>(
                name: "ItemPortalMultiplier",
                table: "ClickerPlayers",
                type: "double",
                nullable: false,
                defaultValue: 1.0);

            migrationBuilder.AddColumn<double>(
                name: "ItemPriceMultiplier",
                table: "ClickerPlayers",
                type: "double",
                nullable: false,
                defaultValue: 1.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseClickAmount",
                table: "ClickerPlayers");

            migrationBuilder.DropColumn(
                name: "ClickMultiplier",
                table: "ClickerPlayers");

            migrationBuilder.DropColumn(
                name: "ItemPortalMultiplier",
                table: "ClickerPlayers");

            migrationBuilder.DropColumn(
                name: "ItemPriceMultiplier",
                table: "ClickerPlayers");
        }
    }
}
