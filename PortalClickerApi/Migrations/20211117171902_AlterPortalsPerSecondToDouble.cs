using Microsoft.EntityFrameworkCore.Migrations;

namespace PortalClickerApi.Migrations
{
    public partial class AlterPortalsPerSecondToDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PortalsPerSecond",
                table: "ClickerPlayers",
                type: "double",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned");

            migrationBuilder.AlterColumn<double>(
                name: "PortalCount",
                table: "ClickerPlayers",
                type: "double",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ulong>(
                name: "PortalsPerSecond",
                table: "ClickerPlayers",
                type: "bigint unsigned",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<ulong>(
                name: "PortalCount",
                table: "ClickerPlayers",
                type: "bigint unsigned",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }
    }
}
