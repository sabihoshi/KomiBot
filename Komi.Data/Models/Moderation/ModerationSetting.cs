using Komi.Data.Models.Discord.Guild;
using Komi.Data.Models.Settings;

namespace Komi.Data.Models.Moderation
{
    public class ModerationSetting : IGroupSetting
    {
        public ulong ModerationSettingId { get; set; }

        public int? KickAt { get; set; }

        public int? BanAt { get; set; }

        public ulong GroupId { get; set; }

        public Group Group { get; set; }
    }
}