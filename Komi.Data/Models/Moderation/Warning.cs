using System;
using Komi.Data.Models.Users;

namespace Komi.Data.Models.Moderation
{
    public class Warning
    {
        public Guid Id { get; set; }

        public User User { get; set; }

        public User Mod { get; set; }

        public int Count { get; set; }

        public string? Reason { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}