using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;

namespace PortalClickerApi.Database.Models
{
    public class ClickerSystemItem : DbEntity
    {
        public ICollection<ClickerUserItem> UserItems { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CostExpression { get; set; }
        public double Portals { get; set; }

        public static ulong CalculateCost(string costExpression, ulong amount)
        {
            var function = new Function($"calc(amount) = {costExpression}");
            var expression = new Expression($"calc({amount})", function);
            var result = expression.calculate();
            return (ulong)result;
        }
    }
}
