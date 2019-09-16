using System.Collections.Generic;

namespace Komi.Data.Models.Settings
{
    public class GuildSettings : IGuildData
    {
        public ulong Id { get; set; }

        public string ModName { get; set; }

        public List<string> Prefixes { get; set; }
    }
}