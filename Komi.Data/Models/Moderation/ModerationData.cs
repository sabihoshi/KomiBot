using System.Collections.Generic;
using Komi.Data.Models.Settings;

namespace Komi.Data.Models.Moderation
{
    public class ModerationData : IGuildData
    {
        public ulong Id { get; set; }

        public List<WarningData> Warnings { get; set; } = new List<WarningData>();
    }
}