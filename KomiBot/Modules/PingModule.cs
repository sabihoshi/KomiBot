using System.Threading.Tasks;
using Discord.Commands;

namespace KomiBot.Modules
{
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Alias("pong")]
        [Summary("Shows the current ping of the bot.")]
        public Task PingAsync()
        {
            return ReplyAsync($"Pong! {Context.Client.Latency}");
        }
    }
}