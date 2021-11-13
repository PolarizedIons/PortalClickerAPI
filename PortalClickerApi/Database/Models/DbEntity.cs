using System;
using System.ComponentModel.DataAnnotations;

namespace PortalClickerApi.Database.Models
{
    public class DbEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
