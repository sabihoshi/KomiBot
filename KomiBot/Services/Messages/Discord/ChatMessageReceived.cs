using Discord;
using MediatR;

namespace KomiBot.Services.Messages.Discord
{
    public class ChatMessageReceived : INotification
    {
        public IMessage Message { get; set; }
    }
}
