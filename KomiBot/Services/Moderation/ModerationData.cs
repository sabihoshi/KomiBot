using System.Collections.Generic;
using KomiBot.Services.Guild;

namespace KomiBot.Services.Moderation
{
    public class ModerationData : IGuildData
    {
        public int Id { get; set; }

        public ulong GuildId { get; set; }

        public List<WarningData> Warnings { get; set; } = new List<WarningData>();
    }
}