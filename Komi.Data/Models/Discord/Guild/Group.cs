using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Komi.Data.Models.Discord.User;
using Komi.Data.Models.Moderation;
using Komi.Data.Models.Settings;
using Komi.Data.Models.Tracking.Scanlation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Komi.Data.Models.Discord.Guild
{
    public interface IGroup
    {
        ulong GroupId { get; set; }
    }

    public class Group
        : IGroup
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong GroupId { get; set; }

        public List<Series> Projects { get; set; }

        public List<GroupMember> Members { get; set; }

        public GroupSetting GroupSettings { get; set; } = new GroupSetting();

        public ModerationSetting? ModerationSettings { get; set; }

        public ModerationData? ModerationData { get; set; }
    }

    public class GroupMember : IUser, IGroup
    {
        public int GroupMemberId { get; set; }

        public ulong GroupId { get; set; }

        public Group Group { get; set; }

        [ForeignKey(nameof(Member))]
        public ulong UserId { get; set; }

        public Member Member { get; set; }
    }

    public class GroupSetup : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder
               .HasOne(x => x.ModerationData)
               .WithOne(x => x.Group)
               .HasForeignKey<ModerationData>(x => x.GroupId);
        }
    }
}