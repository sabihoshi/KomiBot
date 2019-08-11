﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using KomiBot.Services.Messages.Discord;
using MediatR;

namespace KomiBot.Services.Help
{
    public class CommandErrorHandler :
        INotificationHandler<ReactionAdded>,
        INotificationHandler<ReactionRemoved>
    {
        //This relates user messages with errors
        private readonly Dictionary<ulong, string> _associatedErrors = new Dictionary<ulong, string>();
        //This relates user messages to modix messages containing errors
        private readonly Dictionary<ulong, ulong> _errorReplies = new Dictionary<ulong, ulong>();

        private const string _emoji = "⚠";

        /// <summary>
        /// Associates a user message with an error
        /// </summary>
        /// <param name="message">The message containing an errored command</param>
        /// <param name="error">The error that occurred</param>
        /// <returns></returns>
        public async Task AssociateError(IUserMessage message, string error)
        {
            await message.AddReactionAsync(new Emoji(_emoji));
            _associatedErrors.Add(message.Id, error);
        }

        public Task Handle(ReactionAdded notification, CancellationToken cancellationToken)
            => ReactionAdded(notification.Message, notification.Channel, notification.Reaction);

        public async Task ReactionAdded(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            //Don't trigger if the emoji is wrong, if the user is a bot, or if we've
            //made an error message reply already

            if (reaction.User.IsSpecified && reaction.User.Value.IsBot)
            {
                return;
            }

            if (reaction.Emote.Name != _emoji || _errorReplies.ContainsKey(cachedMessage.Id))
            {
                return;
            }

            //If the message that was reacted to has an associated error, reply in the same channel
            //with the error message then add that to the replies collection
            if (_associatedErrors.TryGetValue(cachedMessage.Id, out var value))
            {
                var msg = await channel.SendMessageAsync("", false, new EmbedBuilder()
                {
                    Author = new EmbedAuthorBuilder
                    {
                        IconUrl = "https://raw.githubusercontent.com/twitter/twemoji/gh-pages/2/72x72/26a0.png",
                        Name = "That command had an error"
                    },
                    Description = value,
                    Footer = new EmbedFooterBuilder { Text = "Remove your reaction to delete this message" }
                }.Build());

                _errorReplies.Add(cachedMessage.Id, msg.Id);
            }
        }

        public Task Handle(ReactionRemoved notification, CancellationToken cancellationToken)
            => ReactionRemoved(notification.Message, notification.Channel, notification.Reaction);

        public async Task ReactionRemoved(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            //Bugfix for NRE?
            if (reaction is null || reaction.User.Value is null)
            {
                return;
            }

            //Don't trigger if the emoji is wrong, or if the user is bot
            if (reaction.User.IsSpecified && reaction.User.Value.IsBot)
            {
                return;
            }

            if (reaction.Emote.Name != _emoji)
            {
                return;
            }

            //If there's an error reply when the reaction is removed, delete that reply,
            //remove the cached error, remove it from the cached replies, and remove
            //all reactions from the original messages
            if (_errorReplies.TryGetValue(cachedMessage.Id, out var botReplyId))
            {
                var msg = await channel.GetMessageAsync(botReplyId);
                await msg.DeleteAsync();

                _associatedErrors.Remove(cachedMessage.Id);
                _errorReplies.Remove(cachedMessage.Id);

                var originalMessage = await cachedMessage.GetOrDownloadAsync();
                await originalMessage.RemoveAllReactionsAsync();
            }
        }
    }
}
