using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using JetBrains.Annotations;
using KomiBot.Core.Attributes;
using KomiBot.Services.Core;
using KomiBot.Services.Moderation;
using KomiBot.Services.Settings;

namespace KomiBot.Modules
{
    [Group("Settings")]
    [Alias("Setting")]
    public class SettingsModule : ModuleBase<SocketCommandContext>
    {
        [UsedImplicitly] public DatabaseService DatabaseService { get; set; }

        [UsedImplicitly] public SettingsService SettingsService { get; set; }

        [Command("keys")]
        [Summary("View all the available keys")]
        [UsedImplicitly]
        public Task ViewKeysAsync(Settings settings)
        {
            var embed = GetKeysEmbed(settings);

            return embed is null
                ? ReplyAsync("Settings not found")
                : ReplyAsync(embed: embed.Build());
        }

        [Command]
        [Summary("View the configured settings of the server")]
        [UsedImplicitly]
        public Task ViewSettingsAsync(Settings settings)
        {
            var embed = GetSettingsEmbed(settings);

            return embed is null
                ? ReplyAsync("Settings not found")
                : ReplyAsync(embed: embed.Build());
        }

        private EmbedBuilder? GetSettingsEmbed(Settings settings)
        {
            var embed = new EmbedBuilder()
               .WithTitle($"Keys of {settings.ToString()}");

            if (settings == Settings.Guild)
            {
                if (!DatabaseService.TryGetGuildData(Context.Guild, out GuildSettings guildSettings))
                    return null;
                return embed.WithDescription(AppendProperties<GuildSettings>(guildSettings).ToString());
            }

            if (settings == Settings.Moderation)
            {
                if (!DatabaseService.TryGetGuildData(Context.Guild, out ModerationSettings moderationSettings))
                    return null;
                return embed.WithDescription(AppendProperties<ModerationSettings>(moderationSettings).ToString());
            }

            return null;
        }

        private StringBuilder AppendProperties<T>(IGuildData data)
        {
            var sb = new StringBuilder();

            foreach (var property in SettingsService.GetProperties<T>())
                sb.AppendLine($"{Format.Bold(property.Name)}: {property.GetValue(data)}");

            return sb;
        }

        private EmbedBuilder? GetKeysEmbed(Settings settings)
        {
            var embed = new EmbedBuilder();

            var keys = settings switch
            {
                Settings.Guild => SettingsService.GetProperties<GuildSettings>(),
                Settings.Moderation => SettingsService.GetProperties<ModerationSettings>(),
                _ => null
            };

            if (keys is null)
                return null;

            embed.WithTitle($"Keys of {settings.ToString()}")
                 .WithDescription(AppendKeys(keys).ToString());

            return embed;
        }

        private static StringBuilder AppendKeys(IReadOnlyCollection<PropertyInfo> keys)
        {
            var sb = new StringBuilder();

            foreach (var key in keys)
                sb.AppendLine($"{Format.Bold(key.Name)}: {key.GetCustomAttribute<DescriptionAttribute>()?.Text}");

            return sb;
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