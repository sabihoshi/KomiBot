using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using KomiBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KomiBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                  .AddSingleton<DiscordSocketClient>()
                  .AddSingleton<CommandService>()
                  .AddSingleton<ApplicationService>()
                  .AddSingleton<CommandHandlingService>()
                  .BuildServiceProvider();
        }

        private Task LogAsync(LogMessage message)
        {
            Console.WriteLine(message.Message);
            return Task.CompletedTask;
        }

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();
                var application = services.GetRequiredService<ApplicationService>();

                // Events
                client.Log += LogAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

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