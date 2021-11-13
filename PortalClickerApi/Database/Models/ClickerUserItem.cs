using org.mariuszgromada.math.mxparser;

namespace PortalClickerApi.Database.Models
{
    public class ClickerUserItem : DbEntity
    {
        public ClickerSystemItem SystemItem { get; set; }
        public ulong Amount { get; set; }
        
        public ulong NextCost
        {
            get
            {
                var function = new Function($"calc(amount) = {SystemItem.CostExpression}");
                var expression = new Expression($"calc({Amount})", function);
                var result = expression.calculate();
                return (ulong)result;
            }
        }
    }
}