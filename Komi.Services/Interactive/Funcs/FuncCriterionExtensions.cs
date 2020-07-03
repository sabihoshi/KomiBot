using System;
using Discord.WebSocket;

namespace Komi.Services.Interactive.Funcs
{
    public static class FuncCriterionExtensions
    {
        public static FuncCriterion AsCriterion(this Func<SocketMessage, bool> func) => new FuncCriterion(func);
    }
}