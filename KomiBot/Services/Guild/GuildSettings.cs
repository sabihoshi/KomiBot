namespace KomiBot.Services.Guild
{
    public class GuildSettings : IGuildData
    {
        public int Id { get; set; }

        public ulong GuildId { get; set; }

        public string ModName { get; set; }
    }
}