namespace KomiBot.Services.Settings
{
    public class GuildSettings : IGuildData
    {
        public ulong Id { get; set; }

        public string ModName { get; set; }
    }
}