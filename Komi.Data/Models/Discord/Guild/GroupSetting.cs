using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Komi.Data.Models.Tracking;

namespace Komi.Data.Models.Settings
{
    public class GroupSetting : IGroupSetting
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong GroupId { get; set; }

        public List<Prefix> Prefixes { get; set; } = new List<Prefix>();

        public List<WorkTypeSetting> DefaultWorkTypes { get; set; } =
            WorkTypeExtensions.Default
               .Select(x => new WorkTypeSetting(x))
               .ToList();

        public ulong TrackingChannel { get; set; }

        public int? KickAt { get; set; }

        public int? BanAt { get; set; }
    }

    public class WorkTypeSetting
    {
        public WorkTypeSetting(WorkType workType) => WorkType = workType;

        public int WorkTypeSettingId { get; set; }

        public WorkType WorkType { get; set; }
    }

    public class Prefix
    {
        public int PrefixId { get; set; }

        public string Text { get; set; }
    }
}