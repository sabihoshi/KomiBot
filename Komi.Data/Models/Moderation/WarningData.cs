using System;
using System.ComponentModel.DataAnnotations;
using Komi.Data.Models.Discord.Guild;

namespace Komi.Data.Models.Moderation
{
    public class WarningData
    {
        [Key]
        public ulong WarningId { get; set; }

        public ulong UserId { get; set; }

        public ulong ModId { get; set; }

        public int Count { get; set; }

        public string? Reason { get; set; }

        public DateTimeOffset Date { get; set; }

        public Group Group { get; set; }

        public ulong GroupId { get; set; }
    }
}