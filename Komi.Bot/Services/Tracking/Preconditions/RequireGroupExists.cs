using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Komi.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Komi.Bot.Services.Tracking.Preconditions
{
    public class RequireGroupExists : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(
            ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var db = services.GetRequiredService<KomiContext>();
            return db.Groups.SingleOrDefault(g => g.GuildId == context.Guild.Id) is null
                ? PreconditionResult.FromError("Group does not exist")
                : PreconditionResult.FromSuccess();
        }
    }
}