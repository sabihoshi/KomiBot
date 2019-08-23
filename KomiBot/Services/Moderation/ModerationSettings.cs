﻿using System.Collections.Generic;
using KomiBot.Services.Settings;

namespace KomiBot.Services.Moderation
{
    public class ModerationSettings : IGuildData
    {
        public ulong Id { get; set; }

        public int? KickAt { get; set; }

        public int? BanAt { get; set; }

        public List<ulong> Moderators { get; set; } = new List<ulong>();

        public List<ulong> ModeratorRoles { get; set; } = new List<ulong>();
    }
}