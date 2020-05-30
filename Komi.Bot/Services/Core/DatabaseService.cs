using Komi.Data;

namespace Komi.Bot.Services.Core
{
    public class DatabaseService
        : IDatabaseService
    {
        public KomiContext CreateDbContext()
        {
            var factory = new KomiContextFactory();

            return factory.CreateDbContext(new string[] { });
        }
    }
}