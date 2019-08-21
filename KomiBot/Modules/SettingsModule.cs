using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using JetBrains.Annotations;
using KomiBot.Services.Core;
using KomiBot.Services.Guild;
using KomiBot.Services.Moderation;
using KomiBot.Services.Utilities;
using KomiBot.TypeReaders;
using LiteDB;

namespace KomiBot.Modules
{
    [Group("Settings")]
    [Alias("Setting")]
    public class SettingsModule : ModuleBase<SocketCommandContext>
    {
        private IReadOnlyCollection<PropertyInfo>? _guildSettingKeys;
        private IReadOnlyCollection<PropertyInfo>? _moderationSettingKeys;

        private Type _cachedType;

        [UsedImplicitly] public DatabaseService DatabaseService { get; set; }

        [Command("keys")]
        [Summary("View all the available keys")]
        public Task ViewKeysAsync(Settings settings)
        {
            var embed = GetKeysEmbed(settings);

            return embed is null
                ? ReplyAsync("Settings not found")
                : ReplyAsync(embed: embed.Build());
        }

        [Command]
        [Summary("View the configured settings of the server")]
        public Task ViewSettingsAsync(Settings settings)
        {
            var embed = GetSettingsEmbed(settings);

            return embed is null
                ? ReplyAsync("Settings not found")
                : ReplyAsync(embed: embed.Build());
        }

        [Command]
        [Summary("Set a key setting")]
        public Task SetSettingsAsync(Settings settings, string key, string value)
        {
            var result = settings switch
            {
                Settings.Guild => TrySetSettingAsync<GuildSettings>(GetType<GuildSettings>(), key,
                    value),
                Settings.Moderation => TrySetSettingAsync<ModerationSettings>(
                    GetType<ModerationSettings>(), key, value),
                _ => false
            };

            return result
                ? ReplyAsync("Updated key.")
                : ReplyAsync("That key was not found.");
        }

        public bool TrySetSettingAsync<T>(Type type, string key, string value) where T : class, IGuildData, new()
        {
            if (!DatabaseService.TryGetProperty(type, key, value, out var property, out var newValue))
                return false;

            var (collection, data) = GetSettings<T>(type);

            property?.SetValue(data, newValue);
            collection.Upsert(data);

            return true;
        }

        public (LiteCollection<T>, T) GetSettings<T>(Type type) where T : class, IGuildData, new()
        {
            return (DatabaseService.GetTableData<T>(type.Name), DatabaseService.EnsureGuildData<T>(Context.Guild, type.Name));
        }

        private EmbedBuilder? GetSettingsEmbed(Settings settings)
        {
            var embed = new EmbedBuilder()
               .WithTitle($"Keys of {settings.ToString()}");

            if (settings == Settings.Guild)
            {
                if (!DatabaseService.TryGetGuildData(Context.Guild, out GuildSettings guildSettings))
                    return null;
                return embed.WithDescription(AppendProperties(guildSettings).ToString());
            }

            if (settings == Settings.Moderation)
            {
                if (!DatabaseService.TryGetGuildData(Context.Guild, out ModerationSettings moderationSettings))
                    return null;
                return embed.WithDescription(AppendProperties(moderationSettings).ToString());
            }

            return null;
        }

        private static StringBuilder AppendProperties(IGuildData data)
        {
            var sb = new StringBuilder();

            foreach (var property in data.GetType()
                                         .GetProperties()
                                         .Where(k => k.Attributes.GetAttributeOfType<HiddenAttribute>() is null))
                sb.AppendLine($"{Format.Bold(property.Name)}: {property.GetValue(data)}");

            return sb;
        }

        private EmbedBuilder? GetKeysEmbed(Settings settings)
        {
            var embed = new EmbedBuilder();

            var keys = settings switch
            {
                Settings.Guild => GetKeys<GuildSettings>(ref _guildSettingKeys,
                    GetType<GuildSettings>()),
                Settings.Moderation => GetKeys<ModerationSettings>(ref _moderationSettingKeys,
                    GetType<ModerationSettings>()),
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

            foreach (var key in keys.Where(k => k.Attributes.GetAttributeOfType<HiddenAttribute>() is null))
                sb.AppendLine($"{Format.Bold(key.Name)}: {key.GetDescription()}");

            return sb;
        }

        private IReadOnlyCollection<PropertyInfo> GetKeys<T>(ref IReadOnlyCollection<PropertyInfo>? cachedKeys,
            Type type)
        {
            return LazyInitializer.EnsureInitialized(ref cachedKeys, () =>
            {
                var keys = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                               .Where(p => p.Name != "Id")
                               .ToList();
                return keys;
            });
        }

        private Type GetType<T>()
        {
            return LazyInitializer.EnsureInitialized(ref _cachedType, () => typeof(T));
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