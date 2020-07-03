﻿using System.Threading.Tasks;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;

namespace Komi.Services.Interactive.TryParse
{
    public class TryParseCriterion<T> : ICriterion<SocketMessage>
    {
        private readonly TryParseDelegate<T> _tryParse;

        public TryParseCriterion(TryParseDelegate<T> tryParse) => _tryParse = tryParse;

        public Task<bool> JudgeAsync(SocketCommandContext sourceContext, SocketMessage parameter) =>
            Task.FromResult(_tryParse(parameter.Content, out _));
    }
}