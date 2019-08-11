using Discord.Commands;

namespace KomiBot.TypeReaders
{
    [NamedArgumentType]
    public class ReasonArguments
    {
        public string? Reason { get; set; }
    }
}