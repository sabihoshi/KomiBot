using Discord;
using MediatR;

namespace KomiBot.Services.Messages.Discord
{
    public class UserLeft : INotification
    {
        public IGuild Guild { get; set; }
    }
}
