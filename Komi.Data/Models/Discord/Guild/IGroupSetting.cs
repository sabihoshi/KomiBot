using Komi.Data.Models.Discord.Guild;

namespace Komi.Data.Models.Settings
{
    public interface IGroupSetting
    {
        public ulong GroupId { get; set; }

        public Group Group { get; set; }
    }
}