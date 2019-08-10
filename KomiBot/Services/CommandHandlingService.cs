using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace KomiBot.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;
            _discord.MessageReceived += MessageReceivedAsync;
        }

        public Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified)
                return Task.CompletedTask;

            if (result.IsSuccess)
                return Task.CompletedTask;

            return context.Channel.SendMessageAsync($"error: {result}");
        }

        public Task InitializeAsync()
        {
            return _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message)) return Task.CompletedTask;
            if (message.Source != MessageSource.User) return Task.CompletedTask;

            var argPos = 0;
            if (!(message.HasStringPrefix("k!", ref argPos) ||
                  message.HasMentionPrefix(_discord.CurrentUser, ref argPos))) return Task.CompletedTask;

            var context = new SocketCommandContext(_discord, message);
            return _commands.ExecuteAsync(context, argPos, _services);
        }
    }
}