using Discord;

namespace KomiBot.Services.Guild
{
    public interface IGuildData
    {
        int Id { get; set; }
        ulong GuildId { get; set; }
    }
}