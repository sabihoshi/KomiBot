using System;
using Komi.Data.Models.Tracking;

namespace Komi.Data.Models.Settings
{
    public class WorkTypeSetting
    {
        public WorkTypeSetting(WorkType workType) => WorkType = workType;

        public Guid Id { get; set; }

        public WorkType WorkType { get; set; }
    }
}