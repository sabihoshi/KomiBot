using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Komi.Data.Models.Users;

namespace Komi.Data.Models.Tracking
{
    public class Worker : IUser
    {
        [Key]
        [ForeignKey(nameof(User))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }

        public Status Status { get; set; }

        public DateTimeOffset Started { get; set; }

        public DateTimeOffset Finished { get; set; }

        public Job Job { get; set; }

        public User User { get; set; }
    }
}