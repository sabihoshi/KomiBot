using System.Collections.Generic;

namespace KomiBot.Models.GuildData.Data
{
    public class ModerationData : IGuildData
    {
        public ulong GuildId { get; set; }

        public List<WarningData> Warnings { get; set; } = new List<WarningData>();
    }
}