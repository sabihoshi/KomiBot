using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Komi.Data.Models.Moderation;
using Komi.Data.Models.Settings;
using Komi.Data.Models.Tracking.Scanlation;
using Komi.Data.Models.Users;

namespace Komi.Data.Models.Groups
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong GuildId { get; set; }

        public List<Series> Projects { get; set; } = new List<Series>();

        public List<GroupMember> Members { get; set; } = new List<GroupMember>();

        public GroupSetting GroupSettings { get; set; } = new GroupSetting();

        public List<Warning> Warnings { get; set; } = new List<Warning>();
    }
}