using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PortalClickerApi.Migrations
{
    public partial class AddLastClick : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastClick",
                table: "ClickerPlayers",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastClick",
                table: "ClickerPlayers");
        }
    }
}
