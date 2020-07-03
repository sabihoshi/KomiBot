using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Komi.Data;
using Komi.Services.Core.Messages;
using MediatR;

namespace Komi.Services.Tracking
{
    public class TrackingListener : INotificationHandler<ReactionAddedNotification>
    {
        private readonly KomiContext _context;

        public async Task Handle(ReactionAddedNotification notification, CancellationToken cancellationToken)
        {
            if(notification.Channel is ITextChannel channel)
            {
                //TODO: Update state of the work
                var group = _context
                   .Groups.SingleOrDefault(x => x.GuildId == channel.GuildId);
            }
        }
    }
}
