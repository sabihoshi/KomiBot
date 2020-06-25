using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using JetBrains.Annotations;
using Komi.Bot.Core.Attributes;
using Komi.Bot.Services.Image;
using Komi.Bot.Services.Utilities;
using Komi.Data;
using Komi.Data.Models.Settings;

namespace Komi.Bot.Modules
{
    [Group("Settings")]
    [Alias("Setting")]
    public class SettingsModule : ModuleBase<SocketCommandContext>
    {
        [UsedImplicitly] public KomiContext Db { get; set; }

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
                //Settings.Group when DatabaseService.TryGetGuildData(
                //    Context.Group, out Data.Models.Settings.GroupSetting guildSettings)
                //=> embed.WithDescription(AppendProperties<Data.Models.Settings.GroupSetting>(guildSettings)),
                //Settings.Moderation when DatabaseService.TryGetGuildData(
                //    Context.Group, out ModerationSetting moderationSettings)
                //=> embed.WithDescription(AppendProperties<ModerationSetting>(moderationSettings)),
                _ => null
            };
        }

        private string AppendProperties<T>(IGroupSetting data) where T : IGroupSetting
        {
            var sb = new StringBuilder();

            foreach (var property in CacheExtensions.GetProperties<T>())
            {
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType)
                 && property.PropertyType != typeof(string))
                {
                    var enumerable = (property.GetValue(data) as IEnumerable)?.OfType<object>()
                                  ?? Enumerable.Empty<object>();

                    sb.AppendLine($"{Format.Bold(property.Name)}:");
                    sb.AppendLine("```prolog\n"
                                + $"{string.Join("\n", enumerable)}"
                                + "```");
                }
                else
                    sb.AppendLine($"{Format.Bold(property.Name)}: {property.GetValue(data) ?? 0}");
            }

            return sb.ToString();
        }

        private async Task<EmbedBuilder?> GetKeysEmbed(Settings settings)
        {
            var embed = new EmbedBuilder();

            var keys = settings switch
            {
                Settings.Guild => CacheExtensions.GetProperties<GroupSetting>(),

                //Settings.Moderation => CacheExtensions.GetProperties<ModerationSetting>(),
                _ => null
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