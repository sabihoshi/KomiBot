using System.Collections.Generic;
using System.Linq;
using Discord.Commands;
using Komi.Data.Models.Settings;

namespace Komi.Data.Models.Moderation
{
    public class ModerationSettings : IGuildData
    {
        public ulong Id { get; set; }

        public int? KickAt { get; set; }

        public int? BanAt { get; set; }

        public List<ulong> Moderators { get; set; } = new List<ulong>();

        public List<ulong> ModeratorRoles { get; set; } = new List<ulong>();
    }
}