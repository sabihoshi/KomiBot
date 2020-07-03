using System;
using Discord.Commands;
using Komi.Services.Core.Attributes;

namespace Komi.Services.Core.TypeReaders
{
    [NamedArgumentType]
    public class TimedReasonArguments
    {
        [Description("Number of days of messages to prune. [0-7]")]
        public TimeSpan Time { get; set; } = TimeSpan.Zero;

        [Description("Reason to be written to Audit Log.")]
        public string? Reason { get; set; } = string.Empty;
    }
}