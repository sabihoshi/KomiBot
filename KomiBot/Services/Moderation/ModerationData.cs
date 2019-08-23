using System.Collections.Generic;
using KomiBot.Services.Settings;

namespace KomiBot.Services.Moderation
{
    public class ModerationData : IGuildData
    {
        public ulong Id { get; set; }

        public List<WarningData> Warnings { get; set; } = new List<WarningData>();
    }
}