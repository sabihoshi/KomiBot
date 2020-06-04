using Komi.Data.Models.Discord.Guild;
using Komi.Data.Models.Discord.User;
using Komi.Data.Models.Tracking.Scanlation;
using Microsoft.EntityFrameworkCore;

namespace Komi.Data
{
    public class KomiContext : DbContext
    {
        public KomiContext(DbContextOptions<KomiContext> options) : base(options) { }

        public DbSet<Group> Groups { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Work> Works { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(KomiContext).Assembly);
        }
    }
}