using System;
using System.Collections.Generic;
using Komi.Data.Models.Discord.Guild;
using Komi.Data.Models.Discord.User;

namespace Komi.Data.Models.Tracking
{
    public class Worker : IUser
    {
        public ulong WorkerId { get; set; }

        public Status Status { get; set; }

        public DateTimeOffset Started { get; set; }

        public DateTimeOffset Finished { get; set; }

        public Job Job { get; set; }

        public ulong UserId { get; set; }

        public List<Group> Groups { get; set; }
    }
}