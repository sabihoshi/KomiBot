using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Komi.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Komi.Services.Tracking.Preconditions
{
    public class RequireGroupMemberOf : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(
            ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var db = services.GetRequiredService<KomiContext>();
            var group = db.Groups.SingleOrDefault(g => g.GuildId == context.Guild.Id);

            if (group is null)
                return PreconditionResult.FromError("Group does not exist");

            return group.Members.Any(member => member.User.Id == context.User.Id)
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError("You're not a member of this group");
        }
    }
}