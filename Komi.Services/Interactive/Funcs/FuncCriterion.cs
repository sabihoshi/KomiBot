﻿using System;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;

namespace Komi.Services.Interactive.Funcs
{
    public class FuncCriterion : ICriterion<SocketMessage>
    {
        private readonly Func<SocketMessage, bool> _func;

        public FuncCriterion(Func<SocketMessage, bool> func) => _func = func;

        public Task<bool> JudgeAsync(SocketCommandContext sourceContext, SocketMessage parameter) =>
            Task.FromResult(_func(parameter));
    }
}