using Komi.Data.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Komi.Data
{
    public class KomiContextFactory : IDesignTimeDbContextFactory<KomiContext>
    {
        public KomiContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .AddUserSecrets<KomiContextFactory>()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<KomiContext>()
               .UseSqlite(configuration.GetValue<string>(nameof(KomiConfig.DbConnection)));

            return new KomiContext(optionsBuilder.Options);
        }
    }
}