using System;
using System.Collections.Generic;
using PortalClickerApi.Identity;

namespace PortalClickerApi.Database.Models
{
    public class ClickerPlayer : DbEntity
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ulong PortalCount { get; set; }
        public ulong PortalsPerSecond { get; set; }
        public DateTime? LastTick { get; set; }
        public DateTime? LastClick { get; set; }

        public ulong BaseClickAmount { get; set; }
        public double ClickMultiplier { get; set; }
        public double ItemPriceMultiplier { get; set; }
        public double ItemPortalMultiplier { get; set; }

        public ICollection<ClickerUpgrade> Upgrades { get; set; }
        public ICollection<ClickerUserItem> Items { get; set; }
    }
}
