using System.Collections.Generic;
using PortalClickerApi.Migrations;
using PortalClickerApi.Models;

namespace PortalClickerApi.Database.Models
{
    public class ClickerUpgrade : DbEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ActionText { get; set; }
        public ulong Price { get; set; }

        public UpgradeMultiplierType MultiplierType { get; set; }
        public double MultiplierAmount { get; set; }

        public ICollection<ClickerPlayer> Players { get; set; }
    }
}
