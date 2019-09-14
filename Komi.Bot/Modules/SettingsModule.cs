using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using JetBrains.Annotations;
using Komi.Bot.Core.Attributes;
using Komi.Bot.Services.Core;
using Komi.Bot.Services.Image;
using Komi.Bot.Services.Settings;
using Komi.Bot.Services.Utilities;
using Komi.Data.Models.Moderation;
using Komi.Data.Models.Settings;

namespace Komi.Bot.Modules
{
    [Group("Settings")]
    [Alias("Setting")]
    public class SettingsModule : ModuleBase<SocketCommandContext>
    {
        [UsedImplicitly] public DatabaseService DatabaseService { get; set; }

        [UsedImplicitly] public IImageService ImageService { get; set; }

        [Command("keys")]
        [Summary("View all the available keys")]
        [UsedImplicitly]
        public async Task ViewKeysAsync(Settings settings)
        {
            var embed = await GetKeysEmbed(settings);

            await (embed is null ? ReplyAsync("Settings not found") : ReplyAsync(embed: embed.Build()));
        }

        [Command]
        [Summary("View the configured settings of the server")]
        [UsedImplicitly]
        public async Task ViewSettingsAsync(Settings settings)
        {
            var embed = await GetSettingsEmbed(settings);

            await (embed is null ? ReplyAsync("Settings not found") : ReplyAsync(embed: embed.Build()));
        }

        private async Task<EmbedBuilder?> GetSettingsEmbed(Settings settings)
        {
            var embed = new EmbedBuilder().WithColor(await GetAvatarColor(Context.User))
               .WithTitle($"Keys of {settings.ToString()}");

            return settings switch
            {
                Settings.Guild when DatabaseService.TryGetGuildData(
                    Context.Guild, out GuildSettings guildSettings)
                => embed.WithDescription(AppendProperties<GuildSettings>(guildSettings)),
                Settings.Moderation when DatabaseService.TryGetGuildData(
                    Context.Guild, out ModerationSettings moderationSettings)
                => embed.WithDescription(AppendProperties<ModerationSettings>(moderationSettings)),
                _ => null
            };
        }

        private string AppendProperties<T>(IGuildData data)
        {
            var sb = new StringBuilder();

            foreach (var property in CacheExtensions.GetProperties<T>())
                sb.AppendLine($"{Format.Bold(property.Name)}: {property.GetValue(data)}");

            return sb.ToString();
        }

        private async Task<EmbedBuilder?> GetKeysEmbed(Settings settings)
        {
            var embed = new EmbedBuilder();

            var keys = settings switch
            {
                Settings.Guild      => CacheExtensions.GetProperties<GuildSettings>(),
                Settings.Moderation => CacheExtensions.GetProperties<ModerationSettings>(),
                _                   => null
            };

            if (keys is null)
                return null;

            embed.WithTitle($"Keys of {settings.ToString()}")
               .WithColor(await GetAvatarColor(Context.User))
               .WithDescription(AppendKeys(keys).ToString());

            return embed;
        }

        private static StringBuilder AppendKeys(IReadOnlyCollection<PropertyInfo> keys)
        {
            var sb = new StringBuilder();

            foreach (var key in keys)
                sb.AppendLine($"{Format.Bold(key.Name)}: {key.GetAttribute<DescriptionAttribute>()?.Text}");

            return sb;
        }

        private ValueTask<Color> GetAvatarColor(IUser contextUser)
        {
            ValueTask<Color> colorTask = default;

            if ((contextUser.GetAvatarUrl(size: 16) ?? contextUser.GetDefaultAvatarUrl()) is { } avatarUrl)
                colorTask = ImageService.GetDominantColorAsync(new Uri(avatarUrl));

            return colorTask;
        }

        public enum Settings
        {
            [Description("Request the guild settings")]
            Guild,

            [Description("Request the moderation settings")]
            Moderation
        }
    }
}