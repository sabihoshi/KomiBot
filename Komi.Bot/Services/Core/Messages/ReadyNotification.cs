﻿using MediatR;

namespace Komi.Bot.Services.Core.Messages
{
    /// <summary>
    ///     Describes an application-wide notification that occurs when <see cref="IDiscordSocketClient.Ready" /> is raised.
    /// </summary>
    public class ReadyNotification : INotification
    {
        /// <summary>
        ///     A default, reusable instance of the <see cref="ReadyNotification" /> class.
        /// </summary>
        public static readonly ReadyNotification Default
            = new ReadyNotification();

        private ReadyNotification() { }
    }
}