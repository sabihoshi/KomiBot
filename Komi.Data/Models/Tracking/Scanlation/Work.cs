using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Komi.Data.Models.Groups;

namespace Komi.Data.Models.Tracking.Scanlation
{
    public class Work : IGroup
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public double? Volume { get; set; }

        public double Chapter { get; set; }

        public Group Group { get; set; }

        public Status? OverridenStatus { get; set; }

        [NotMapped]
        public Status Status => OverridenStatus ?? Jobs.Select(x => x.Status).GetStatus();

        public List<Job> Jobs { get; set; }
    }
}