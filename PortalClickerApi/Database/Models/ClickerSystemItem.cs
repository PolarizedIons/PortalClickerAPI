namespace PortalClickerApi.Database.Models
{
    public class ClickerSystemItem : DbEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CostExpression { get; set; }
    }
}