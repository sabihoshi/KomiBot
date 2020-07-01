using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Komi.Bot.Services.Core;
using Komi.Bot.Services.Core.Listeners;
using Komi.Bot.Services.Help;
using Komi.Bot.Services.Image;
using Komi.Bot.Services.Tracking;
using Komi.Data;
using Komi.Data.Models.Core;
using MangaDexApi;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Komi.Bot
{
    public class Program
    {
        public Program() =>
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .CreateLogger();

        public static async Task Main(string[] args) { await new Program().MainAsync(); }

        private static ServiceProvider ConfigureServices() =>
            new ServiceCollection().AddHttpClient()
               .AddMemoryCache()
               .AddDbContext<KomiContext>(OptionConfiguration, ServiceLifetime.Transient)
               .AddMediatR(c => c.Using<KomiMediator>(), typeof(Program))
               .AddLogging(l => l.AddSerilog(dispose: true))
               .AddSingleton<InteractiveService>()
               .AddSingleton<DiscordSocketClient>()
               .AddSingleton<CommandService>()
               .AddSingleton<CommandHandlingService>()
               .AddSingleton<GroupService>()
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

            options.UseNpgsql(configuration.GetValue<string>(nameof(KomiConfig.DbConnection)));
        }

        private Task LogAsync(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    Log.Fatal(message.ToString());
                    break;
                case LogSeverity.Error:
                    Log.Error(message.ToString());
                    break;
                case LogSeverity.Warning:
                    Log.Warning(message.ToString());
                    break;
                case LogSeverity.Info:
                    Log.Information(message.ToString());
                    break;
                case LogSeverity.Verbose:
                    Log.Verbose(message.ToString());
                    break;
                case LogSeverity.Debug:
                    Log.Debug(message.ToString());
                    break;
            }

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
                //commands.RegisterSetting<GroupSetting>();
                //commands.RegisterSetting<ModerationSetting>();

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