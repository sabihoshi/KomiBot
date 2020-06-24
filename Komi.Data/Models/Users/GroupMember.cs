using System;
using Komi.Data.Models.Groups;

namespace Komi.Data.Models.Users
{
    public class GroupMember : IUser, IGroup
    {
        public Guid Id { get; set; }

        public Group Group { get; set; }

        public User User { get; set; }
    }
}