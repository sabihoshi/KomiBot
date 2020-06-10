using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Komi.Data.Models.Discord.Guild;
using Komi.Data.Models.Settings;

namespace Komi.Data.Models.Moderation
{
    public class ModerationSetting : IGroupSetting
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong GroupId { get; set; }

        public int? KickAt { get; set; }

        public int? BanAt { get; set; }
    }
}