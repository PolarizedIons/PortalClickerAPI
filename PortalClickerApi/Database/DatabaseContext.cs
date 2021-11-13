using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortalClickerApi.Database.Models;
using PortalClickerApi.Identity;

namespace PortalClickerApi.Database
{
    public sealed class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<ClickerPlayer> ClickerPlayers { get; set; }
        public DbSet<ClickerSystemItem> SystemItems { get; set; }
        public DbSet<ClickerUpgrade> Upgrades { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            ChangeTracker.Tracked += OnEntityTracked;
        }

        private static void OnEntityTracked(object? sender, EntityTrackedEventArgs e)
        {
            if (e.Entry.State == EntityState.Added && e.Entry.Entity is DbEntity entity)
            {
                entity.Id = Guid.NewGuid();
                entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }
}
