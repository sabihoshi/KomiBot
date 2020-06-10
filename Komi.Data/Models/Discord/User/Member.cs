using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Komi.Data.Models.Discord.Guild;

namespace Komi.Data.Models.Discord.User
{
    public class Member : IUser
    {
        [Key]
        [ForeignKey(nameof(User))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong UserId { get; set; }

        public User User { get; set; }

        public List<GroupMember> Groups { get; set; }
    }
}