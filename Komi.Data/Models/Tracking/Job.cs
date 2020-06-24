using System;
using System.Collections.Generic;

namespace Komi.Data.Models.Tracking
{
    public class Job
    {
        public Job(WorkType type) => Type = type;

        public Guid Id { get; set; }

        public WorkType Type { get; set; }

        public List<Worker> Workers { get; set; } = new List<Worker>();

        public Status Status { get; set; } = Status.Unknown;
    }
}