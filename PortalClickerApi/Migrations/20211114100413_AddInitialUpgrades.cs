using System;
using Microsoft.EntityFrameworkCore.Migrations;
using PortalClickerApi.Models;

namespace PortalClickerApi.Migrations
{
    public partial class AddInitialUpgrades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "Upgrades",
                new string[]
                {
                    "Id", "Name", "Description", "Price", "CreatedAt", "ActionText", "MultiplierAmount", "MultiplierType"
                },
                new object[]
                {
                    "071E353E-6488-47DA-9646-E0CDA5CA79E2",
                    "Orange Portal",
                    "This next test might take a very, very long time", 
                    300,
                    DateTime.UtcNow,
                    "+1 base portal per click.",
                    1, 
                    (int)UpgradeMultiplierType.AddClick
                }
            );

            migrationBuilder.InsertData(
                "Upgrades",
                new string[]
                {
                    "Id", "Name", "Description", "Price", "CreatedAt", "ActionText", "MultiplierAmount", "MultiplierType"
                },
                new object[]
                {
                    "84237AB6-1333-4F4C-9F18-D8306ADD7064", 
                    "Splitscreen Co-op",
                    "Twice the players, twice the portals!",
                    5000,
                    DateTime.UtcNow,
                    "100% more portals from items and clicks.", 
                    2,
                    (int)(UpgradeMultiplierType.Click | UpgradeMultiplierType.ItemPortals)
                }
            );
            migrationBuilder.InsertData(
                "Upgrades",
                new string[]
                {
                    "Id", "Name", "Description", "Price", "CreatedAt", "ActionText", "MultiplierAmount", "MultiplierType"
                },
                new object[]
                {
                    "BDBDF510-D7CE-49B3-8127-88EAD3850432",
                    "Conversion Gel", 
                    "More portalable surfaces!",
                    15000,
                    DateTime.UtcNow,
                    "20% more portals from items and clicks.", 
                    1.2,
                    (int)(UpgradeMultiplierType.Click | UpgradeMultiplierType.ItemPortals)
                }
            );
            migrationBuilder.InsertData(
                "Upgrades",
                new string[]
                {
                    "Id", "Name", "Description", "Price", "CreatedAt", "ActionText", "MultiplierAmount", "MultiplierType"
                },
                new object[]
                {
                    "F7CFE0FD-9866-47D6-BA78-3B04DB49D882",
                    "Perpetual Testing Initiative",
                    "Offload your work to the multiverse!", 
                    50000, 
                    DateTime.UtcNow,
                    "Item prices reduced by 5%",
                    0.95,
                    (int)UpgradeMultiplierType.ItemPrice
                });

            migrationBuilder.InsertData(
                "Upgrades",
                new string[]
                {
                    "Id", "Name", "Description", "Price", "CreatedAt", "ActionText", "MultiplierAmount", "MultiplierType"
                },
                new object[]
                {
                    "B3AC0733-181D-4579-A0AA-8839135E9744",
                    "Out Of Bounds Clip",
                    "Discover portal surfaces past the constraints of the world.", 
                    250000, 
                    DateTime.UtcNow,
                    "10% more portals from items and clicks.",
                    1.10,
                    (int)(UpgradeMultiplierType.Click | UpgradeMultiplierType.ItemPortals)
                }
            );
            migrationBuilder.InsertData(
                "Upgrades",
                new string[]
                {
                    "Id", "Name", "Description", "Price", "CreatedAt", "ActionText", "MultiplierAmount", "MultiplierType"
                },
                new object[]
                {
                    "DF18140D-4EDA-4011-8C64-FD07441E30F3", 
                    "Steam Workshop",
                    "Clicking Subscribe practically downloads more portal surfaces!",
                    30000,
                    DateTime.UtcNow,
                    "10 base portals per click.",
                    10,
                    (int)UpgradeMultiplierType.AddClick
                }
            );
            migrationBuilder.InsertData(
                "Upgrades",
                new string[]
                {
                    "Id", "Name", "Description", "Price", "CreatedAt", "ActionText", "MultiplierAmount", "MultiplierType"
                },
                new object[]
                {
                    "055977C7-CD1F-42CC-9926-663547523FE6",
                    "Cross-platform Play",
                    "How does this work on analog triggers? Do I get analog portals?",
                    750000,
                    DateTime.UtcNow,
                    "+50 base portals per click.",
                    50,
                    (int)UpgradeMultiplierType.AddClick
                }
            );
            migrationBuilder.InsertData(
                "Upgrades",
                new string[]
                {
                    "Id", "Name", "Description", "Price", "CreatedAt", "ActionText", "MultiplierAmount", "MultiplierType"
                },
                new object[]
                {
                    "D60FD0C9-BC91-46C0-8B50-844E958F0F37",
                    "challenge_maplist.txt",
                    "Some extra challenge mode maps for some extra portal surfaces.",
                    2000000,
                    DateTime.UtcNow,
                    "+5% more portals from items and clicks.",
                    1.05,
                    (int)(UpgradeMultiplierType.Click | UpgradeMultiplierType.ItemPortals)
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
