using System;
using Discord;
using Discord.Commands;
using JetBrains.Annotations;
using KomiBot.Services.Guild;
using KomiBot.TypeReaders;

namespace KomiBot.Services.Moderation
{
    public class WarningData : IGuildData
    {
        public WarningData(SocketCommandContext context, IGuildUser user, WarningArguments args)
        {
            Reason = args?.Reason ?? string.Empty;
            Count = args?.Count ?? 1;
            Date = DateTime.UtcNow;
            Id = context.Guild.Id;
            ModId = context.User.Id;
            UserId = user.Id;
        }

        [UsedImplicitly]
        public WarningData() { }

        public ulong UserId { get; set; }

        public ulong ModId { get; set; }

        public ulong Id { get; set; }

        public int Count { get; set; }

        public string? Reason { get; set; }

        public DateTime Date { get; set; }
    }
}