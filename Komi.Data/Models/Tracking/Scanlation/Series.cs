using System.Collections.Generic;

namespace Komi.Data.Models.Tracking.Scanlation
{
    public class Series
    {
        public long SeriesId { get; set; }

        public List<Work> Works { get; set; }
    }
}