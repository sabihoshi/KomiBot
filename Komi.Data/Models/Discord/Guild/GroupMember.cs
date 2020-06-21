using System.ComponentModel.DataAnnotations.Schema;
using Komi.Data.Models.Discord.User;

namespace Komi.Data.Models.Discord.Guild
{
    public class GroupMember : IUser, IGroup
    {
        public long GroupMemberId { get; set; }

        public ulong GroupId { get; set; }

        [ForeignKey(nameof(User))]
        public ulong Id { get; set; }

        public Group Group { get; set; }

        public User.User User { get; set; }
    }
}