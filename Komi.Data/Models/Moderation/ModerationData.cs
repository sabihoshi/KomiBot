using System.Collections.Generic;
using Komi.Data.Models.Discord.Guild;

namespace Komi.Data.Models.Moderation
{
    public class ModerationData
    {
        public ulong ModerationDataId { get; set; }

        public List<WarningData>? Warnings { get; set; } = new List<WarningData>();

        public Group Group { get; set; }

        public ulong GroupId { get; set; }
    }
}