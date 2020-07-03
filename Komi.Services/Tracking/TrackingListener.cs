using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Komi.Bot.Services.Core.Messages;
using Komi.Data;
using MediatR;

namespace Komi.Bot.Services.Tracking
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
