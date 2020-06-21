using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Komi.Data.Models.Moderation;
using Komi.Data.Models.Settings;
using Komi.Data.Models.Tracking.Scanlation;

namespace Komi.Data.Models.Discord.Guild
{
    public class Group
        : IGroup
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong GroupId { get; set; }

        public List<Series> Projects { get; set; } = new List<Series>();

        public List<GroupMember> Members { get; set; } = new List<GroupMember>();

        public GroupSetting GroupSettings { get; set; } = new GroupSetting();

        public ModerationData? ModerationData { get; set; }
    }
}