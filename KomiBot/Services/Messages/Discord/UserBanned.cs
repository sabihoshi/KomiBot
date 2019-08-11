using Discord;
using MediatR;

namespace KomiBot.Services.Messages.Discord
{
    public class UserBanned : INotification
    {
        public IUser BannedUser { get; set; }

        public IGuild Guild { get; set; }
    }
}
