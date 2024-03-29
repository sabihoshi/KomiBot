﻿using Discord.Commands;

namespace Komi.Services.Core.TypeReaders
{
    [NamedArgumentType]
    public class WorkArguments
    {
        public string? Description { get; set; }

        public double? Volume { get; set; }

        public double Chapter { get; set; }
    }
}