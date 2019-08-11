using Discord;
using MediatR;

namespace KomiBot.Services.Messages.Discord
{
    public class UserJoined : INotification
    {
        public IGuild Guild { get; set; }

        public IGuildUser User { get; set; }
    }
}
