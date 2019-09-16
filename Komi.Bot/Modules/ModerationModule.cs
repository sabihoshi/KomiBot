using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using JetBrains.Annotations;
using Komi.Bot.Core.Preconditions;
using Komi.Bot.Core.TypeReaders;
using Komi.Bot.Services.Core;
using Komi.Data.Models.Moderation;

namespace Komi.Bot.Modules
{
    [Name("Moderation")]
    [Summary("Commands for moderation in the server.")]
    public class ModerationModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDatabaseService _db;

        private ModerationSettings? _settings;
        private ModerationData? _data;

        public ModerationModule(IDatabaseService db) => _db = db;

        [Command("ban")]
        [Summary("Bans a user mentioned.\nExample: `k!ban @user time: 5d reason: Spam`")]
        [Priority(10)]
        [RequireContext(ContextType.Guild)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [UsedImplicitly]
        public async Task BanUserAsync(
            [RequireHigherRole] IGuildUser user,
            TimedReasonArguments? args = null)
        {
            var mod = (IGuildUser)Context.User;
            _settings = _db.EnsureGuildData<ModerationSettings>(Context.Guild);

            if (!IsModerator(_settings, mod) 
             && !mod.GuildPermissions.Has(GuildPermission.BanMembers))
            {
                await ReplyAsync("You don't have permissions to ban.");
                return;
            }

            if (args == null)
                await user.BanAsync();
            else
                await user.BanAsync(args.Time.Days, args.Reason);

            await ReplyAsync($"{user.Username}#{user.Discriminator} was banned.");
        }

        [Command("kick")]
        [Summary("Kick a user mentioned")]
        [Priority(10)]
        [RequireContext(ContextType.Guild)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        [UsedImplicitly]
        public async Task KickUserAsync(
            [RequireHigherRole] IGuildUser user)
        {
            var mod = (IGuildUser)Context.User;
            _settings = _db.EnsureGuildData<ModerationSettings>(Context.Guild);

            if (!IsModerator(_settings, mod) 
             && !mod.GuildPermissions.Has(GuildPermission.KickMembers))
            {
                await ReplyAsync("You don't have permissions to kick.");
                return;
            }

            await user.KickAsync();
            await ReplyAsync($"{mod.Username}#{mod.Discriminator} was kicked.");
        }

        [Command("warn")]
        [Summary("Warns a user mentioned")]
        [RequireContext(ContextType.Guild)]
        [UsedImplicitly]
        public async Task WarnUser(
            [RequireHigherRole] IGuildUser user,
            WarningArguments? args = null)
        {
            _settings = _db.EnsureGuildData<ModerationSettings>(Context.Guild);
            if (!IsModerator(_settings, (IGuildUser)Context.User))
            {
                await ReplyAsync("You don't have permissions to warn.");
                return;
            }

            var warning = new WarningData
            {
                UserId = user.Id,
                ModId = Context.User.Id,
                Count = args?.Count ?? 1,
                Reason = args?.Reason,
                Date = DateTimeOffset.UtcNow
            };

            _data = _db.EnsureGuildData<ModerationData>(Context.Guild);
            _data.Warnings.Add(warning);

            if (ShouldBe(Sanction.Ban, user))
                await BanUserAsync(user, new TimedReasonArguments { Reason = args?.Reason });
            else if (ShouldBe(Sanction.Kick, user))
                await KickUserAsync(user);
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
                Sanction.Ban  => _settings?.BanAt,
                _             => null
            };

            if (sanctionAt is null || sanctionAt == 0)
                return false;

            return _data?.Warnings
                      .Where(w => w.UserId == subject.Id)
                      .Sum(w => w.Count)
                 > sanctionAt;
        }

        private bool IsModerator(
            ModerationSettings settings,
            IGuildUser user)
        {
            return settings
                      .Moderators
                      .Contains(user.Id)
                || settings
                      .ModeratorRoles
                      .Any(role => user.RoleIds.Contains(role));
        }
    }
}