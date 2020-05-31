using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading;
using Discord.Commands;
using Discord.Commands.Builders;
using Komi.Bot.Core.Attributes;
using Komi.Bot.Services.Utilities;
using Komi.Data;
using Komi.Data.Models.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Komi.Bot.Services.Settings
{
    public static class SettingsSetup
    {
        private static KomiContext? _db;

        public static void RegisterSetting<T>(this CommandService commands) where T : class, IGroupSetting, new()
        {
            string group = typeof(T).Name.ToLower().Replace("Settings", string.Empty);

            commands.CreateModuleAsync("Settings", module =>
            {
                foreach (var property in CacheExtensions.GetPrimitives<T>())
                {
                    module.CreateCommand<T>("Updated key.", group, property.Name, property, (p, s, args) =>
                        p.SetValue(s, args.First()));
                }

                foreach (var property in CacheExtensions.GetLists<T>())
                {
                    string name = property.Name;
                    module.CreateCommand<T>("Cleared key.", group, $"clear {name}", property, (p, s, args) =>
                        (p.GetValue(property) as IList)?.Clear());

                    module.CreateCommand<T>("Added key.", group, name, property, (p, s, args) =>
                    {
                        var list = p.GetValue(property) as IList;
                        foreach (var item in args)
                            list?.Add(item);
                    });
                }
            });
        }

        private static void CreateCommand<TData>(
            this ModuleBuilder module,
            string message, string group, string key,
            PropertyInfo property, Action<PropertyInfo, TData, object[]> propertyFunc)
            where TData : class, IGroupSetting, new()
        {
            module.AddCommand(key, (ctx, args, service, command) =>
            {
                GetData(ctx, args, service, property, propertyFunc);
                return ctx.Channel.SendMessageAsync(message);
            }, c => c.WithSettings(property.GetRealType(), group, key));
            module.AddAttributes(new HiddenAttribute());
        }

        private static void WithSettings(
            this CommandBuilder command, Type type,
            string group, string key)
        {
            command.WithSummary($"Sets the {key} key in {group}");
            command.AddParameter("Value", type, p =>
            {
                p.AddAttributes(new RemainderAttribute());
                p.WithSummary("The new value of the key");
            });
        }

        private static void GetData<TData>(
            ICommandContext ctx, object[] args,
            IServiceProvider service, PropertyInfo property,
            Action<PropertyInfo, TData, object[]> propertyFunc)
            where TData : class, IGroupSetting, new()
        {
            var db = GetDatabaseService(service);
            var settings = db.Find<TData>(ctx.Guild.Id) ?? new TData();
            propertyFunc.Invoke(property, settings, args);
            db.Update(settings);
            db.SaveChanges();
        }

        private static KomiContext GetDatabaseService(IServiceProvider services) =>
            LazyInitializer.EnsureInitialized(ref _db, services.GetRequiredService<KomiContext>);
    }
}