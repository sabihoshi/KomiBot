using Discord;
using MediatR;

namespace KomiBot.Services.Messages.Modix
{
    public class RemovableMessageRemoved : INotification
    {
        public IMessage Message { get; set; }
    }
}
