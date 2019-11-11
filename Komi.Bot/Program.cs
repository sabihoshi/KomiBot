using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Komi.Bot.Services.Core;
using Komi.Bot.Services.Help;
using Komi.Bot.Services.Image;
using Komi.Bot.Services.Settings;
using Komi.Data.Models.Moderation;
using Komi.Data.Models.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Komi.Bot
{
    public class Program
    {
        public static void Main(string[] args) { new Program().MainAsync().GetAwaiter().GetResult(); }

        private ServiceProvider ConfigureServices() =>
            new ServiceCollection().AddHttpClient()
               .AddMemoryCache()
               .AddSingleton<DiscordSocketClient>()
               .AddSingleton<CommandService>()
               .AddSingleton<CommandHandlingService>()
               .AddSingleton<ApplicationService>()
               .AddScoped<IFunModuleService, FunModuleService>()
               .AddScoped<IDatabaseService, DatabaseService>()
               .AddCommandHelp()
               .AddImages()
               .BuildServiceProvider();

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
                var application = services.GetRequiredService<ApplicationService>();
                var commands = services.GetRequiredService<CommandService>();

                // Custom Commands
                commands.RegisterSetting<GuildSettings>();
                commands.RegisterSetting<ModerationSettings>();

                // Events
                client.Log += LogAsync;
                commands.Log += LogAsync;

                // Login
                await client.LoginAsync(TokenType.Bot, application.Token);
                await client.StartAsync();

                // Here we initialize the logic required to register our commands.
                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                // Block this task until the program is closed.
                await Task.Delay(-1);
            }
        }
    }
}