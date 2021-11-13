using System.Collections.Generic;

namespace PortalClickerApi.Database.Models
{
    public class ClickerUpgrade : DbEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ulong Price { get; set; }
        
        public ICollection<ClickerPlayer> Players { get; set; }
    }
}
