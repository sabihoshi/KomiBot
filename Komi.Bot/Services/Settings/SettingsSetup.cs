using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading;
using Discord.Commands;
using Discord.Commands.Builders;
using Komi.Bot.Core.Attributes;
using Komi.Bot.Services.Core;
using Komi.Bot.Services.Utilities;
using Komi.Data.Models.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Komi.Bot.Services.Settings
{
    public static class SettingsSetup
    {
        private static IDatabaseService? _databaseService;

        public static void RegisterSetting<T>(this CommandService commands) where T : class, IGuildData, new()
        {
            string guildData = typeof(T).Name.ToLower().Replace("Settings", string.Empty);

            commands.CreateModuleAsync("Settings", module =>
            {
                foreach (var property in CacheExtensions.GetPrimitives<T>())
                {
                    CreateCommand<T>("Updated key.", module, guildData, property, property.Name, (p, s, args) =>
                        p.SetValue(s, args.First()));
                }

                foreach (var property in CacheExtensions.GetLists<T>())
                {
                    string name = property.Name;
                    CreateCommand<T>("Cleared key.", module, guildData, property, name, (p, s, args) =>
                        (p.GetValue(property) as IList)?.Clear());

                    CreateCommand<T>("Added key.", module, guildData, property, name, (p, s, args) =>
                    {
                        var list = p.GetValue(property) as IList;
                        foreach (var item in args)
                            list?.Add(item);
                    });
                }
            });

            static void CreateCommand<TData>(string message, ModuleBuilder module, string data, PropertyInfo property,
                string name, Action<PropertyInfo, TData, object[]> propertyFunc) where TData : class, IGuildData, new()
            {
                module.AddCommand(name, (ctx, args, service, command) =>
                {
                    GetData<TData>(ctx, service, property, args, propertyFunc);
                    return ctx.Channel.SendMessageAsync(message);
                }, c => WithSettings(c, name, property, data));
                module.AddAttributes(new HiddenAttribute());
            }

            static void WithSettings(
                CommandBuilder command, string propName,
                PropertyInfo property, string keyName)
            {
                command.WithSummary($"Sets the {propName} key in {keyName}");
                command.AddParameter("Value", property.GetRealType(), p =>
                {
                    p.AddAttributes(new RemainderAttribute());
                    p.WithSummary("The new value of the key");
                });
            }

            static void GetData<TData>(
                ICommandContext ctx, IServiceProvider service,
                PropertyInfo property, object[] args,
                Action<PropertyInfo, TData, object[]> propertyFunc)
                where TData : class, IGuildData, new()
            {
                var db = GetDatabaseService(service);
                var settings = db.EnsureGuildData<TData>(ctx.Guild);
                propertyFunc.Invoke(property, settings, args);
                db.UpdateData(settings);
            }
        }

        private static IDatabaseService GetDatabaseService(IServiceProvider services) =>
            LazyInitializer.EnsureInitialized(ref _databaseService, services.GetRequiredService<IDatabaseService>);
    }
}