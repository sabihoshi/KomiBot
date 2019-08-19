using LiteDB;

namespace KomiBot.Services.Guild
{
    public class GuildSettings : IGuildData
    {
        public ulong Id { get; set; }

        public string ModName { get; set; }
    }
}