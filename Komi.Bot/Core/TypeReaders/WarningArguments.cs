using Discord.Commands;

namespace Komi.Bot.Core.TypeReaders
{
    [NamedArgumentType]
    public class WarningArguments
    {
        public string Reason { get; set; } = string.Empty;

        public int Count { get; set; } = 1;
    }
}