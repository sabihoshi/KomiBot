using System;
using Discord.Commands;

namespace KomiBot.TypeReaders
{
    [NamedArgumentType]
    public class TimedReasonArguments
    {
        public TimeSpan Time { get; set; } = TimeSpan.Zero;

        public string Reason { get; set; } = string.Empty;
    }
}