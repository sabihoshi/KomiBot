using System.Collections.Generic;
using Discord;
using Komi.Data.Models.Tracking;

namespace Komi.Services.Tracking
{
    public class WorkStatusEmote
    {
        public WorkStatusEmote(WorkType type, string grey, string yellow, string green, string red)
        {
            Type = type;
            Emotes = new Dictionary<StatusColor, Emote>
            {
                [StatusColor.Grey] = Emote.Parse(grey),
                [StatusColor.Yellow] = Emote.Parse(yellow),
                [StatusColor.Red] = Emote.Parse(red),
                [StatusColor.Green] = Emote.Parse(green)
            };
        }

        public enum StatusColor
        {
            Grey,
            Yellow,
            Red,
            Green
        }

        public WorkType Type { get; }

        public Dictionary<StatusColor, Emote> Emotes { get; }
    }
}