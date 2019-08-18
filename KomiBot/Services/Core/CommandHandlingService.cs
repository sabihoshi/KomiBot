﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace KomiBot.Services.Core
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services, CommandService commands, DiscordSocketClient discord)
        {
            _commands = commands;
            _discord = discord;
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

            // return CommandFailedAsync(context, result);
            return Task.CompletedTask;
        }

        public Task CommandFailedAsync(ICommandContext context, IResult result)
        {
            return context.Channel.SendMessageAsync($"Error: {result.ErrorReason}");
        }

        public Task InitializeAsync()
        {
            return _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            var argPos = 0;
            if (!(message.HasStringPrefix("k!", ref argPos) ||
                  message.HasMentionPrefix(_discord.CurrentUser, ref argPos))) return;

            var context = new SocketCommandContext(_discord, message);
            var result = await _commands.ExecuteAsync(context, argPos, _services);

            if (!result.IsSuccess) await CommandFailedAsync(context, result).ConfigureAwait(false);
        }
    }
}