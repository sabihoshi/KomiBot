using System.Collections.Generic;
using KomiBot.Services.Guild;

namespace KomiBot.Services.Moderation
{
    public class ModerationSettings : IGuildData
    {
        public int Id { get; set; }

        public ulong GuildId { get; set; }

        public int? KickAt { get; set; }

        public int? BanAt { get; set; }

        public List<ulong> Moderators { get; set; } = new List<ulong>();

        public List<ulong> ModeratorRoles { get; set; } = new List<ulong>();
    }
}