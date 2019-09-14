using System.Collections.Generic;
using Komi.Bot.Services.Settings;

namespace Komi.Bot.Services.Moderation
{
    public class ModerationData : IGuildData
    {
        public ulong Id { get; set; }

        public List<WarningData> Warnings { get; set; } = new List<WarningData>();
    }
}