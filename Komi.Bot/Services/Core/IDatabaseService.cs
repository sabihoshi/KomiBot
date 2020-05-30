using Komi.Data;

namespace Komi.Bot.Services.Core
{
    public interface IDatabaseService
    {
        KomiContext CreateDbContext();
    }
}