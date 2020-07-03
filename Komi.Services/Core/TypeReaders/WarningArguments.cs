using Discord.Commands;

namespace Komi.Services.Core.TypeReaders
{
    [NamedArgumentType]
    public class WarningArguments
    {
        public string Reason { get; set; } = string.Empty;

        public int Count { get; set; } = 1;
    }
}