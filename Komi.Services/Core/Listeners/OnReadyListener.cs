using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Komi.Data;
using Komi.Services.Core.Messages;
using MediatR;

namespace Komi.Services.Core.Listeners
{
    public class OnReadyListener : INotificationHandler<ReadyNotification>
    {
        public OnReadyListener(KomiContext context, DiscordSocketClient client)
        {
            _context = context;
            _client = client;
        }

        private readonly KomiContext _context;
        private readonly DiscordSocketClient _client;

        public async Task Handle(ReadyNotification notification, CancellationToken cancellationToken)
        {
            return;
            //var missingGuilds =
            //    _client.Guilds.Where(guild =>
            //        _context.Groups
            //           .All(group => group.GuildId != guild.Id));

            //foreach (var guild in missingGuilds)
            //{
            //    if (cancellationToken.IsCancellationRequested)
            //        break;

            //    var moderationSettings = new ModerationSetting();
            //    var groupSettings = new GroupSetting();

            //    var group = new Group
            //    {
            //        GuildId = guild.Id,
            //        Projects = new List<Series>(),
            //        GroupSettings = groupSettings,
            //        ModerationSettings = moderationSettings
            //    };

            //    _context.Groups.Add(group);
            //}

            //await _context.SaveChangesAsync(cancellationToken);
        }
    }
}