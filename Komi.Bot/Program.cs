using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Komi.Data;
using Komi.Data.Models.Core;
using Komi.Services.Core;
using Komi.Services.Core.Listeners;
using Komi.Services.Help;
using Komi.Services.Image;
using Komi.Services.Tracking;
using MangaDexApi;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Komi.Bot
{
    public class Program
    {
        private static ServiceProvider ConfigureServices() =>
            new ServiceCollection().AddHttpClient()
               .AddMemoryCache()
               .AddDbContext<KomiContext>(OptionConfiguration, ServiceLifetime.Transient)
               .AddMediatR(c => c.Using<KomiMediator>(), typeof(Program), typeof(KomiMediator))
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

        private static Task LogAsync(LogMessage message)
        {
            var severity = message.Severity switch
            {
                LogSeverity.Critical => LogEventLevel.Fatal,
                LogSeverity.Error    => LogEventLevel.Error,
                LogSeverity.Warning  => LogEventLevel.Warning,
                LogSeverity.Info     => LogEventLevel.Information,
                LogSeverity.Verbose  => LogEventLevel.Verbose,
                LogSeverity.Debug    => LogEventLevel.Debug,
                _                    => LogEventLevel.Information
            };

            Log.Write(severity, message.Exception, message.Message);

            return Task.CompletedTask;
        }

        public static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .CreateLogger();

            await using var services = ConfigureServices();
            var client = services.GetRequiredService<DiscordSocketClient>();
            var commands = services.GetRequiredService<CommandService>();
            var mediator = services.GetRequiredService<IMediator>();

            var config = new ConfigurationBuilder()
               .AddUserSecrets<KomiConfig>()
               .Build();

            // Custom Commands
            //commands.RegisterSetting<GroupSetting>();
            //commands.RegisterSetting<ModerationSetting>();

            // Events
            var listener = new DiscordSocketListener(client, mediator);
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