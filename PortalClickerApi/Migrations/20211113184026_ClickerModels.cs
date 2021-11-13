using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PortalClickerApi.Migrations
{
    public partial class ClickerModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClickerPlayers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PortalCount = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    PortalsPerSecond = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    LastTick = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClickerPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClickerPlayers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SystemItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CostExpression = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemItems", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Upgrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upgrades", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClickerUserItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SystemItemId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Amount = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    ClickerPlayerId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClickerUserItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClickerUserItem_ClickerPlayers_ClickerPlayerId",
                        column: x => x.ClickerPlayerId,
                        principalTable: "ClickerPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClickerUserItem_SystemItems_SystemItemId",
                        column: x => x.SystemItemId,
                        principalTable: "SystemItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClickerPlayerClickerUpgrade",
                columns: table => new
                {
                    PlayersId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UpgradesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClickerPlayerClickerUpgrade", x => new { x.PlayersId, x.UpgradesId });
                    table.ForeignKey(
                        name: "FK_ClickerPlayerClickerUpgrade_ClickerPlayers_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "ClickerPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClickerPlayerClickerUpgrade_Upgrades_UpgradesId",
                        column: x => x.UpgradesId,
                        principalTable: "Upgrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClickerPlayerClickerUpgrade_UpgradesId",
                table: "ClickerPlayerClickerUpgrade",
                column: "UpgradesId");

            migrationBuilder.CreateIndex(
                name: "IX_ClickerPlayers_UserId",
                table: "ClickerPlayers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClickerUserItem_ClickerPlayerId",
                table: "ClickerUserItem",
                column: "ClickerPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ClickerUserItem_SystemItemId",
                table: "ClickerUserItem",
                column: "SystemItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClickerPlayerClickerUpgrade");

            migrationBuilder.DropTable(
                name: "ClickerUserItem");

            migrationBuilder.DropTable(
                name: "Upgrades");

            migrationBuilder.DropTable(
                name: "ClickerPlayers");

            migrationBuilder.DropTable(
                name: "SystemItems");
        }
    }
}
