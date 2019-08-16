using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using JetBrains.Annotations;

namespace KomiBot.TypeReaders
{
    [NamedArgumentType]
    public class WarningArguments
    {
        public string Reason { get; set; } = string.Empty;

        public int Count { get; set; } = 1;
    }
}
