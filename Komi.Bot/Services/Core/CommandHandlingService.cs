using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Komi.Bot.Services.Core
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandlingService(
            IServiceProvider services,
            CommandService commands)
        {
            _commands = commands;
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;
        }

        public Task CommandExecutedAsync(
            Optional<CommandInfo> command,
            ICommandContext context,
            IResult result)
        {
            if (!command.IsSpecified)
                return Task.CompletedTask;

            if (result.IsSuccess)
                return Task.CompletedTask;

            // return CommandFailedAsync(context, result);
            return Task.CompletedTask;
        }

        public Task InitializeAsync() => _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }
}