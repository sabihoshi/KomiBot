using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using KomiBot.Core.Attributes;
using KomiBot.Services.Core;
using KomiBot.Services.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace KomiBot.Services.Settings
{
    public static class SettingsSetup
    {
        private static DatabaseService? _databaseService;

        public static Task RegisterSetting<T>(this CommandService commands, IServiceProvider services)
            where T : class, IGuildData, new()
        {
            var properties = CacheExtensions.GetProperties<T>();
            var typeName = typeof(T).Name.ToLower().Replace("settings", string.Empty);

            return commands.CreateModuleAsync($"Settings {typeName}", module =>
            {
                foreach (var property in properties)
                {
                    var name = property.Name.ToLower();
                    module.AddCommand(name, (ctx, args, service, command) =>
                        {
                            var db = GetDatabaseService(service);
                            var settings = db.EnsureGuildData<T>(ctx.Guild);
                            var collection = db.GetTableData<T>();
                            property.SetValue(settings, args.First());
                            collection.Update(settings);

                            return ctx.Channel.SendMessageAsync("Updated key.");
                        },
                        command =>
                        {
                            command.WithSummary($"Sets the {name} key in {typeName}");
                            command.AddParameter("value", property.GetRealType(),
                                p => p.AddAttributes(new RemainderAttribute()));
                        });
                }

                module.AddAttributes(new HiddenAttribute());
            });
        }

        private static DatabaseService GetDatabaseService(IServiceProvider services)
        {
            return LazyInitializer.EnsureInitialized(ref _databaseService,
                services.GetRequiredService<DatabaseService>);
        }
    }
}