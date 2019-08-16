using System.Collections.Generic;

namespace KomiBot.Models.GuildData.Settings
{
    public class ModerationSettings : IGuildData
    {
        public ulong GuildId { get; set; }

        public int? KickAt { get; set; }

        public int? BanAt { get; set; }

        public List<ulong> Moderators { get; set; } = new List<ulong>();

        public List<ulong> ModeratorRoles { get; set; } = new List<ulong>();
    }
}