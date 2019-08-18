using System;
using System.Collections.Generic;
using System.Data;
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
        public Task SetSettingsAsync(Settings settings, string key, object value)
        {
            switch (settings)
            {
                case Settings.Guild:
                    return SetKeyAsync(GetSettings<GuildSettings>(), key, value);
                case Settings.Moderation:
                    return SetKeyAsync(GetSettings<ModerationSettings>(), key, value);
                default:
                    return null;
            }
        }

        public ValueTuple<LiteCollection<T>, T> GetSettings<T>() where T : class, IGuildData, new()
        {

            var collection = DatabaseService.GetTableData<T>();
            var data = DatabaseService.EnsureGuildData<T>(Context.Guild);
            return new ValueTuple<LiteCollection<T>, T>(collection, data);
        }

        private Task SetKeyAsync<T>(ValueTuple<LiteCollection<T>, T> tuple, string key, object value)
        {
            var property = tuple.Item2.GetType().GetProperty(key);
            var type = property?.PropertyType;

            if (property is null)
            {
                return ReplyAsync("That key does not exist!");
            }

            try
            {
                var newValue = Convert.ChangeType(value, type);
                property.SetValue(tuple.Item2, newValue);
                tuple.Item1.Upsert(tuple.Item2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ReplyAsync($"An error occured: {e}");
            }

            return ReplyAsync("Changed key!");
        }

        private EmbedBuilder? GetSettingsEmbed(Settings settings)
        {
            var embed = new EmbedBuilder();

            var tableName = settings switch
            {
                Settings.Guild => nameof(GuildSettings),
                Settings.Moderation => nameof(ModerationSettings),
                _ => null
            };

            if (tableName is null)
                return null;

            if (!DatabaseService.TryGetGuildData(Context.Guild, out IGuildData data, tableName))
                return null;

            var type = settings switch
            {
                Settings.Guild => typeof(GuildSettings),
                Settings.Moderation => typeof(ModerationSettings),
                _ => null
            };

            embed.WithTitle($"Keys of {settings.ToString()}")
                 .WithDescription(AppendProperties(data).ToString());

            return embed;
        }

        private static StringBuilder AppendProperties(IGuildData data)
        {
            var sb = new StringBuilder();

            foreach (var property in data.GetType()
                                         .GetProperties()
                                         .Where(k => k.Attributes.GetAttributeOfType<HiddenAttribute>() is null))
            {
                sb.AppendLine($"{Format.Bold(property.Name)}: {property.GetValue(data)}");
            }

            return sb;
        }

        private EmbedBuilder? GetKeysEmbed(Settings settings)
        {
            var embed = new EmbedBuilder();

            var keys = settings switch
            {
                Settings.Guild => GetKeys<GuildSettings>(ref _guildSettingKeys),
                Settings.Moderation => GetKeys<ModerationSettings>(ref _moderationSettingKeys),
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

        private IReadOnlyCollection<PropertyInfo> GetKeys<T>(ref IReadOnlyCollection<PropertyInfo>? cachedKeys)
        {
            return LazyInitializer.EnsureInitialized(ref cachedKeys, () =>
            {
                var keys = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(p => p.Name != "Id")
                                    .ToList();
                return keys;
            });
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