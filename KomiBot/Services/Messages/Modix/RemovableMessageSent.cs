using Discord;
using MediatR;

namespace KomiBot.Services.Messages.Modix
{
    public class RemovableMessageSent : INotification
    {
        public IMessage Message { get; set; }

        public IUser User { get; set; }
    }
}
