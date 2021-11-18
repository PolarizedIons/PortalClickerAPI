using org.mariuszgromada.math.mxparser;
using PortalClickerApi.Identity;

namespace PortalClickerApi.Database.Models
{
    public class ClickerUserItem : DbEntity
    {
        public ClickerPlayer Player { get; set; }
        public ApplicationUser User { get; set; }

        public ClickerSystemItem SystemItem { get; set; }
        public ulong Amount { get; set; }

        public ulong NextCost => ClickerSystemItem.CalculateCost(SystemItem.CostExpression, Amount);
    }
}
