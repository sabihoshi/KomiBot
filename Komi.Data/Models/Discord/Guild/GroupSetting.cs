using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Komi.Data.Models.Discord.Guild;

namespace Komi.Data.Models.Settings
{
    public class GroupSetting : IGroupSetting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong GroupSettingId { get; set; }

        public List<Prefix> Prefixes { get; set; }

        public Group Group { get; set; }

        public ulong GroupId { get; set; }
    }

    public class Prefix
    {
        public long PrefixId { get; set; }

        public string Text { get; set; }
    }
}