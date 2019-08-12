using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using JetBrains.Annotations;
using KomiBot.Preconditions;
using KomiBot.TypeReaders;

namespace KomiBot.Modules
{
    public class ModerationModule : ModuleBase<SocketCommandContext>
    {
        [Command("ban")]
        [Summary("Bans a user mentioned")]
        [Priority(10)]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [UsedImplicitly]
        public async Task BanUserAsync(
            [RequireHigherRole] IGuildUser user,
            [Summary("time: 5h reason: Example")] TimedReasonArguments? args = null)
        {
            if (args == null)
                await user.Guild.AddBanAsync(user);
            else
                await user.Guild.AddBanAsync(user, args.Time.Days, args.Reason);

            await ReplyAsync($"{user.Username}#{user.Discriminator} was banned.");
        }

        [Command("kick")]
        [Summary("Kick a user mentioned")]
        [Priority(10)]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [UsedImplicitly]
        public async Task KickUserAsync(
            [RequireHigherRole] IGuildUser user,
            [Summary("time: 5h reason: Example")] TimedReasonArguments? args = null)
        {
            if (args == null)
                await user.Guild.AddBanAsync(user);
            else
                await user.Guild.AddBanAsync(user, args.Time.Days, args.Reason);

            await ReplyAsync($"{user.Username}#{user.Discriminator} was kicked.");
        }
    }
}