using System.Threading.Tasks;
using Discord.Commands;

namespace KomiBot.Modules
{
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping", true)]
        [Alias("pong")]
        public Task PingAsync()
        {
            return ReplyAsync($"Pong! {Context.Client.Latency}");
        }
    }
}