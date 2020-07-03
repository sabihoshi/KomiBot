using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;

namespace Komi.Bot.Core.TypeReaders
{
    [NamedArgumentType]
    public class WorkArguments
    {
        public string? Description { get; set; }

        public double? Volume { get; set; }

        public double Chapter { get; set; }
    }
}
