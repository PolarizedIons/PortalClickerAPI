using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PortalClickerApi.Migrations
{
    public partial class AddInitialItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "SystemItems",
                new string[]
                {
                    "Id", "Name", "Description", "CostExpression", "CreatedAt", "Portals"
                },
                new object[]
                {
                    "51936D50-EC25-4D46-98D2-E7754BDE6ABF",
                    "Portal Carousel",
                    "It rotates so slowly. How annoying!",
                    "30 * 1.15 ^ amount", 
                    DateTime.UtcNow,
                    0.2
                });
            migrationBuilder.InsertData(
                "SystemItems",
                new string[]
                {
                    "Id", "Name", "Description", "CostExpression", "CreatedAt", "Portals"
                },
                new object[]
                {
                    "0DBB5DA0-E165-4940-8E5C-D458EAAAFBF2",
                    "Pedestal Button",
                    "The true cube and button testing experience.", 
                    "120 * 1.15 ^ amount", 
                    DateTime.UtcNow,
                    1
                });
            migrationBuilder.InsertData(
                "SystemItems",
                new string[]
                {
                    "Id", "Name", "Description", "CostExpression", "CreatedAt", "Portals"
                },
                new object[]
                {
                    "99DE9096-F3F7-4D10-8204-E26B5AF0FE29",
                    "Hold +attack",
                    "One portal every half-second. Useful for timing.", 
                    "200 * 1.15 ^ amount", 
                    DateTime.UtcNow,
                    2
                });
            migrationBuilder.InsertData(
                "SystemItems",
                new string[]
                {
                    "Id", "Name", "Description", "CostExpression", "CreatedAt", "Portals"
                },
                new object[]
                {
                    "98901952-FC3F-406C-A8AB-B5D309512D46",
                    "Reportal Bind",
                    "A portal every 12 ticks. Much easier!", 
                    "650 * 1.15 ^ amount", 
                    DateTime.UtcNow,
                    5
                });
            migrationBuilder.InsertData(
                "SystemItems",
                new string[]
                {
                    "Id", "Name", "Description", "CostExpression", "CreatedAt", "Portals"
                },
                new object[]
                {
                    "487BEA3B-C9E4-44E8-9C22-850D92E036C3",
                    "Portal Bump",
                    "Places portals without moving the crosshair!", 
                    "3000 * 1.15 ^ amount", 
                    DateTime.UtcNow,
                    15
                });
            migrationBuilder.InsertData(
                "SystemItems",
                new string[]
                {
                    "Id", "Name", "Description", "CostExpression", "CreatedAt", "Portals"
                },
                new object[]
                {
                    "F6A66CC5-C5CD-4BBD-9A88-86677A897D3F",
                    "Moving Panels",
                    "The walls move out of the way to free up space for potential portals.", 
                    "40000 * 1.15 ^ amount", 
                    DateTime.UtcNow,
                    50
                });
            migrationBuilder.InsertData(
                "SystemItems",
                new string[]
                {
                    "Id", "Name", "Description", "CostExpression", "CreatedAt", "Portals"
                },
                new object[]
                {
                    "920CAA73-CC8A-406D-BA0F-5EA2EE8207AE",
                    "Higher Sensitivity",
                    "Less time spent aiming means more time spent shooting.", 
                    "450000 * 1.15 ^ amount", 
                    DateTime.UtcNow,
                    250
                });
            migrationBuilder.InsertData(
                "SystemItems",
                new string[]
                {
                    "Id", "Name", "Description", "CostExpression", "CreatedAt", "Portals"
                },
                new object[]
                {
                    "5D68B22A-5AAF-4525-8FB4-940E4E692436",
                    "Placement Helper",
                    "Increases portal placement by reducing sparking. So satisfying.", 
                    "5000000 * 1.15 ^ amount", 
                    DateTime.UtcNow,
                    1250
                });
            migrationBuilder.InsertData(
                "SystemItems",
                new string[]
                {
                    "Id", "Name", "Description", "CostExpression", "CreatedAt", "Portals"
                },
                new object[]
                {
                    "2CF98E25-D412-49FF-8E3A-C211F1368CA0",
                    "SAR Ghost Server",
                    "Get your friends to help you shoot portals! (Note: requires downgrade to 1.12)", 
                    "75000000 * 1.15 ^ amount", 
                    DateTime.UtcNow,
                    7500
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
