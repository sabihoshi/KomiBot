using System;
using KomiBot.Services.Guild;

namespace KomiBot.Services.Moderation
{
    public class WarningData : IGuildData
    {
        public ulong UserId { get; set; }

        public ulong ModId { get; set; }

        public ulong GuildId { get; set; }

        public int Count { get; set; }

        public string Reason { get; set; }

        public DateTime Date { get; set; }
    }
}