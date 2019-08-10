using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using KomiBot.Preconditions;
using KomiBot.Services;
using KomiBot.TypeReaders;

namespace KomiBot.Modules
{
    public class ModerationModule : ModuleBase<SocketCommandContext>
    {
        [Command("ban")]
        [Priority(10)]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync([RequireHigherRole] IGuildUser user, [Remainder] string reason = null)
        {
            await user.Guild.AddBanAsync(user, reason: reason);
            await ReplyAsync($"{user.Username}#{user.Discriminator} was banned.");
        }
    }
}