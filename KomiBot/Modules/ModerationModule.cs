using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using JetBrains.Annotations;
using KomiBot.Models.GuildData.Data;
using KomiBot.Models.GuildData.Settings;
using KomiBot.Preconditions;
using KomiBot.Services;
using KomiBot.TypeReaders;

namespace KomiBot.Modules
{
    [Name("Moderation")]
    [Summary("Commands for moderation in the server.")]
    public class ModerationModule : ModuleBase<SocketCommandContext>
    {
        public DatabaseService? DatabaseService { get; set; }

        private ModerationSettings? _settings;
        private ModerationData? _data;

        [Command("ban")]
        [Summary("Bans a user mentioned.\nExample: `k!ban @user time: 5d reason: Spam`")]
        [Priority(10)]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [UsedImplicitly]
        public async Task BanUserAsync(
            [RequireHigherRole] IGuildUser user,
            TimedReasonArguments? args = null)
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
            TimedReasonArguments? args = null)
        {
            if (args == null)
                await user.Guild.AddBanAsync(user);
            else
                await user.Guild.AddBanAsync(user, args.Time.Days, args.Reason);

            await ReplyAsync($"{user.Username}#{user.Discriminator} was kicked.");
        }

        [Command("warn")]
        [Summary("Warns a user mentioned")]
        [RequireContext(ContextType.Guild)]
        [UsedImplicitly]
        public async Task WarnUser(
            [RequireHigherRole] IGuildUser user,
            WarningArguments? args = null)
        {
            _settings = DatabaseService?.EnsureGuildData<ModerationSettings>(Context.Guild);
            _data = DatabaseService?.EnsureGuildData<ModerationData>(Context.Guild);

            var warning = new WarningData
            {
                Reason = args?.Reason ?? string.Empty,
                Count = args?.Count ?? 1,
                Date = DateTime.UtcNow,
                GuildId = Context.Guild.Id,
                ModId = Context.User.Id,
                UserId = user.Id
            };

            _data?.Warnings.Add(warning);

            if (ShouldBe(Sanction.Ban, user))
                await BanUserAsync(user, new TimedReasonArguments { Reason = args?.Reason });
            else if (ShouldBe(Sanction.Kick, user))
                await KickUserAsync(user, new TimedReasonArguments { Reason = args?.Reason });
        }

        private enum Sanction
        {
            Kick,
            Ban
        }

        private bool ShouldBe(Sanction sanction, IGuildUser subject)
        {
            var sanctionAt = sanction switch
            {
                Sanction.Kick => _settings?.KickAt,
                Sanction.Ban => _settings?.BanAt,
                _ => null
            };

            if (sanctionAt is null || sanctionAt == 0)
                return false;

            return _data?.Warnings
                         .Where(w => w.UserId == subject.Id)
                         .Sum(w => w.Count) > sanctionAt;
        }
    }
}