using System.Threading.Tasks;
using Discord.Commands;
using JetBrains.Annotations;

namespace Komi.Bot.Modules
{
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Alias("pong")]
        [Summary("Shows the current ping of the bot.")]
        [UsedImplicitly]
        public Task PingAsync() => ReplyAsync($"Pong! {Context.Client.Latency}ms");
    }
}