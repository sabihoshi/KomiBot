using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Komi.Data.Models.Discord.Guild;
using Komi.Data.Models.Tracking;

namespace Komi.Data.Models.Settings
{
    public class GroupSetting : IGroupSetting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong GroupSettingId { get; set; }

        public List<Prefix> Prefixes { get; set; }

        public List<WorkTypeSetting> DefaultWorkTypes { get; set; } = new List<WorkTypeSetting>
        {
            new WorkTypeSetting(WorkType.None, false),
            new WorkTypeSetting(WorkType.RawProvider),
            new WorkTypeSetting(WorkType.Translator),
            new WorkTypeSetting(WorkType.Proofreading, false),
            new WorkTypeSetting(WorkType.Cleaning),
            new WorkTypeSetting(WorkType.Redrawing, false),
            new WorkTypeSetting(WorkType.Typesetter),
            new WorkTypeSetting(WorkType.QualityChecker, false),
            new WorkTypeSetting(WorkType.Uploader)
        };

        public ulong TrackingChannel { get; set; }

        public Group Group { get; set; }

        public ulong GroupId { get; set; }
    }

    public class WorkTypeSetting
    {
        public WorkTypeSetting(WorkType workType, bool isEnabled = true)
        {
            WorkType = workType;
            IsEnabled = isEnabled;
        }

        public ulong WorkTypeSettingId { get; set; }

        public WorkType WorkType { get; set; }

        public bool IsEnabled { get; set; }
    }

    public class Prefix
    {
        public long PrefixId { get; set; }

        public string Text { get; set; }
    }
}