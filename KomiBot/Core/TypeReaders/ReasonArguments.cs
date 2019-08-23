using Discord.Commands;

namespace KomiBot.Core.TypeReaders
{
    [NamedArgumentType]
    public class ReasonArguments
    {
        public string? Reason { get; set; }
    }
}