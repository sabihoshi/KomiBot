using Discord.Commands;

namespace Komi.Services.Core.TypeReaders
{
    [NamedArgumentType]
    public class ReasonArguments
    {
        public string? Reason { get; set; }
    }
}