using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PortalClickerApi.Migrations
{
    public partial class UserAndSystemItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClickerUserItem_ClickerPlayers_ClickerPlayerId",
                table: "ClickerUserItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ClickerUserItem_SystemItems_SystemItemId",
                table: "ClickerUserItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClickerUserItem",
                table: "ClickerUserItem");

            migrationBuilder.RenameTable(
                name: "ClickerUserItem",
                newName: "UserItems");

            migrationBuilder.RenameColumn(
                name: "ClickerPlayerId",
                table: "UserItems",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ClickerUserItem_SystemItemId",
                table: "UserItems",
                newName: "IX_UserItems_SystemItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ClickerUserItem_ClickerPlayerId",
                table: "UserItems",
                newName: "IX_UserItems_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerId",
                table: "UserItems",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserItems",
                table: "UserItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_PlayerId",
                table: "UserItems",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserItems_AspNetUsers_UserId",
                table: "UserItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserItems_ClickerPlayers_PlayerId",
                table: "UserItems",
                column: "PlayerId",
                principalTable: "ClickerPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserItems_SystemItems_SystemItemId",
                table: "UserItems",
                column: "SystemItemId",
                principalTable: "SystemItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserItems_AspNetUsers_UserId",
                table: "UserItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserItems_ClickerPlayers_PlayerId",
                table: "UserItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserItems_SystemItems_SystemItemId",
                table: "UserItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserItems",
                table: "UserItems");

            migrationBuilder.DropIndex(
                name: "IX_UserItems_PlayerId",
                table: "UserItems");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "UserItems");

            migrationBuilder.RenameTable(
                name: "UserItems",
                newName: "ClickerUserItem");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ClickerUserItem",
                newName: "ClickerPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_UserItems_UserId",
                table: "ClickerUserItem",
                newName: "IX_ClickerUserItem_ClickerPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_UserItems_SystemItemId",
                table: "ClickerUserItem",
                newName: "IX_ClickerUserItem_SystemItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClickerUserItem",
                table: "ClickerUserItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClickerUserItem_ClickerPlayers_ClickerPlayerId",
                table: "ClickerUserItem",
                column: "ClickerPlayerId",
                principalTable: "ClickerPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClickerUserItem_SystemItems_SystemItemId",
                table: "ClickerUserItem",
                column: "SystemItemId",
                principalTable: "SystemItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
