using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Komi.Bot.Services.Core;
using Komi.Bot.Services.Help;
using Komi.Bot.Services.Image;
using Komi.Bot.Services.Settings;
using Komi.Data;
using Komi.Data.Models.Core;
using Komi.Data.Models.Moderation;
using Komi.Data.Models.Settings;
using MangaDexApi;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Komi.Bot
{
    public class Program
    {
        public static void Main(string[] args) { new Program().MainAsync().GetAwaiter().GetResult(); }

        private static ServiceProvider ConfigureServices() =>
            new ServiceCollection().AddHttpClient()
               .AddMemoryCache()
               .AddDbContext<KomiContext>(OptionConfiguration)
               .AddMediatR(typeof(Program))
               .AddSingleton<DiscordSocketClient>()
               .AddSingleton<CommandService>()
               .AddSingleton<CommandHandlingService>()
               .AddScoped<IFunModuleService, FunModuleService>()
               .AddScoped(MangaDexApiFactory.Create)
               .AddCommandHelp()
               .AddImages()
               .BuildServiceProvider();

        private static void OptionConfiguration(DbContextOptionsBuilder options)
        {
            var configuration = new ConfigurationBuilder()
               .AddUserSecrets<KomiContextFactory>()
               .Build();

            options.UseSqlite(configuration.GetValue<string>(nameof(KomiConfig.DbConnection)));
        }

        private Task LogAsync(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();
                var commands = services.GetRequiredService<CommandService>();
                var mediator = services.GetRequiredService<IMediator>();
                var listener = new DiscordSocketListener(client, mediator);

                var config = new ConfigurationBuilder()
                   .AddUserSecrets<KomiConfig>()
                   .Build();

                // Custom Commands
                commands.RegisterSetting<GroupSetting>();
                commands.RegisterSetting<ModerationSetting>();

                // Events
                await listener.StartAsync(new CancellationToken());
                client.Log += LogAsync;
                commands.Log += LogAsync;

                // Login
                await client.LoginAsync(TokenType.Bot, config.GetValue<string>(nameof(KomiConfig.Token)));
                await client.StartAsync();

                // Here we initialize the logic required to register our commands.
                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                // Block this task until the program is closed.
                await Task.Delay(-1);
            }
        }
    }
}