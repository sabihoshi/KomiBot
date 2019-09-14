using System;
using Komi.Data.Models.Settings;

namespace Komi.Data.Models.Moderation
{
    public class WarningData : IGuildData
    {
        public ulong UserId { get; set; }

        public ulong ModId { get; set; }

        public ulong Id { get; set; }

        public int Count { get; set; }

        public string? Reason { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}