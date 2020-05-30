using Komi.Data.Models.Discord.Guild;
using Microsoft.EntityFrameworkCore;

namespace Komi.Data
{
    public class KomiContext : DbContext
    {
        public KomiContext(DbContextOptions<KomiContext> options) : base(options) { }

        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(KomiContext).Assembly);
        }
    }
}