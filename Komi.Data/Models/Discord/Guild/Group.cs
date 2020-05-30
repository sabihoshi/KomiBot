using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Komi.Data.Models.Moderation;
using Komi.Data.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Komi.Data.Models.Discord.Guild
{
    public class Group
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong GuildId { get; set; }

        [Required]
        public GroupSetting GroupSettings { get; set; }

        [Required]
        public ModerationSetting ModerationSettings { get; set; }

        [Required]
        public ModerationData ModerationData { get; set; }
    }

    public class ModerationDataSetup : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder
               .HasOne(x => x.ModerationData)
               .WithOne(x => x.Group)
               .HasForeignKey<ModerationData>(x => x.GroupId);

            builder
               .HasOne(x => x.GroupSettings)
               .WithOne(x => x.Group)
               .HasForeignKey<GroupSetting>(x => x.GroupId);

            builder
               .HasOne(x => x.ModerationSettings)
               .WithOne(x => x.Group)
               .HasForeignKey<ModerationSetting>(x => x.GroupId);
        }
    }
}