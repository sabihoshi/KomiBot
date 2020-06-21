using Komi.Data.Models.Moderation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Komi.Data.Models.Discord.Guild
{
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