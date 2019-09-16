using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Komi.Bot.Services.Settings;
using Komi.Data.Models.Settings;

namespace Komi.Bot.Services.Core
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly DatabaseService _database;

        public CommandHandlingService(
            IServiceProvider services,
            CommandService commands,
            DiscordSocketClient discord,
            DatabaseService database)
        {
            _commands = commands;
            _discord = discord;
            _services = services;
            _database = database;

            _commands.CommandExecuted += CommandExecutedAsync;
            _discord.MessageReceived += MessageReceivedAsync;
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

        public Task CommandFailedAsync(ICommandContext context, IResult result) =>
            context.Channel.SendMessageAsync($"Error: {result.ErrorReason}");

        public Task InitializeAsync() => _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message))
                return;

            if (message.Source != MessageSource.User)
                return;

            var argPos = 0;
            var context = new SocketCommandContext(_discord, message);
            var settings = _database.EnsureGuildData<GuildSettings>(context.Guild);
            if (!(message.HasStringPrefix("k!", ref argPos)
               || message.HasMentionPrefix(_discord.CurrentUser, ref argPos)))
            {
                if (settings.Prefixes.FirstOrDefault(p => message.HasStringPrefix(p, ref argPos)) is null)
                    return;
            }

            var result = await _commands.ExecuteAsync(context, argPos, _services);

            if (!result.IsSuccess)
                await CommandFailedAsync(context, result).ConfigureAwait(false);
        }
    }
}