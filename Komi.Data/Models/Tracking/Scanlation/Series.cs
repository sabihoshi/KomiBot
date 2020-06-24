using System;
using System.Collections.Generic;

namespace Komi.Data.Models.Tracking.Scanlation
{
    public class Series
    {
        public Guid Id { get; set; }

        public List<Work> Works { get; set; }
    }
}