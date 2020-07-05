using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Komi.Services.Core.Messages;
using MediatR;

namespace Komi.Services.Core
{
    /// <summary>
    ///     Listens for events from an <see cref="IDiscordSocketClient" /> and dispatches them to the rest of the application,
    ///     through an <see cref="IMessageDispatcher" />.
    /// </summary>
    public class DiscordSocketListener
    {
        /// <summary>
        ///     Constructs a new <see cref="DiscordSocketListener" /> with the given dependencies.
        /// </summary>
        public DiscordSocketListener(
            DiscordSocketClient discordSocketClient,
            IMediator messageDispatcher)
        {
            DiscordSocketClient = discordSocketClient;
            MessageDispatcher = messageDispatcher;
        }

        /// <inheritdoc />
        public Task StartAsync(
            CancellationToken cancellationToken)
        {
            DiscordSocketClient.ChannelCreated += OnChannelCreatedAsync;
            DiscordSocketClient.ChannelUpdated += OnChannelUpdatedAsync;
            DiscordSocketClient.GuildAvailable += OnGuildAvailableAsync;
            DiscordSocketClient.JoinedGuild += OnJoinedGuildAsync;
            DiscordSocketClient.MessageDeleted += OnMessageDeletedAsync;
            DiscordSocketClient.MessageReceived += OnMessageReceivedAsync;
            DiscordSocketClient.MessageUpdated += OnMessageUpdatedAsync;
            DiscordSocketClient.ReactionAdded += OnReactionAddedAsync;
            DiscordSocketClient.ReactionRemoved += OnReactionRemovedAsync;
            DiscordSocketClient.Ready += OnReadyAsync;
            DiscordSocketClient.RoleCreated += OnRoleCreatedAsync;
            DiscordSocketClient.RoleUpdated += OnRoleUpdatedAsync;
            DiscordSocketClient.UserBanned += OnUserBannedAsync;
            DiscordSocketClient.UserJoined += OnUserJoinedAsync;
            DiscordSocketClient.UserLeft += OnUserLeftAsync;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(
            CancellationToken cancellationToken)
        {
            DiscordSocketClient.ChannelCreated -= OnChannelCreatedAsync;
            DiscordSocketClient.ChannelUpdated -= OnChannelUpdatedAsync;
            DiscordSocketClient.GuildAvailable -= OnGuildAvailableAsync;
            DiscordSocketClient.JoinedGuild -= OnJoinedGuildAsync;
            DiscordSocketClient.MessageDeleted -= OnMessageDeletedAsync;
            DiscordSocketClient.MessageReceived -= OnMessageReceivedAsync;
            DiscordSocketClient.MessageUpdated -= OnMessageUpdatedAsync;
            DiscordSocketClient.ReactionAdded -= OnReactionAddedAsync;
            DiscordSocketClient.ReactionRemoved -= OnReactionRemovedAsync;
            DiscordSocketClient.Ready -= OnReadyAsync;
            DiscordSocketClient.UserBanned -= OnUserBannedAsync;
            DiscordSocketClient.UserJoined -= OnUserJoinedAsync;
            DiscordSocketClient.UserLeft -= OnUserLeftAsync;

            return Task.CompletedTask;
        }

        /// <summary>
        ///     The <see cref="DiscordSocketClient" /> to be listened to.
        /// </summary>
        protected internal DiscordSocketClient DiscordSocketClient { get; }

        /// <summary>
        ///     A <see cref="IMessageDispatcher" /> used to dispatch discord notifications to the rest of the application.
        /// </summary>
        protected internal IMediator MessageDispatcher { get; }

        private Task OnChannelCreatedAsync(SocketChannel channel)
        {
            MessageDispatcher.Publish(new ChannelCreatedNotification(channel));

            return Task.CompletedTask;
        }

        private Task OnChannelUpdatedAsync(SocketChannel oldChannel, SocketChannel newChannel)
        {
            MessageDispatcher.Publish(new ChannelUpdatedNotification(oldChannel, newChannel));

            return Task.CompletedTask;
        }

        private Task OnGuildAvailableAsync(SocketGuild guild)
        {
            MessageDispatcher.Publish(new GuildAvailableNotification(guild));

            return Task.CompletedTask;
        }

        private Task OnJoinedGuildAsync(SocketGuild guild)
        {
            MessageDispatcher.Publish(new JoinedGuildNotification(guild));

            return Task.CompletedTask;
        }

        private Task OnMessageDeletedAsync(Cacheable<IMessage, ulong> message, ISocketMessageChannel channel)
        {
            MessageDispatcher.Publish(new MessageDeletedNotification(message, channel));

            return Task.CompletedTask;
        }

        private Task OnMessageReceivedAsync(SocketMessage message)
        {
            MessageDispatcher.Publish(new MessageReceivedNotification(message));

            return Task.CompletedTask;
        }

        private Task OnMessageUpdatedAsync(
            Cacheable<IMessage, ulong> oldMessage, SocketMessage newMessage,
            ISocketMessageChannel channel)
        {
            MessageDispatcher.Publish(new MessageUpdatedNotification(oldMessage, newMessage, channel));

            return Task.CompletedTask;
        }

        private Task OnReactionAddedAsync(
            Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel,
            SocketReaction reaction)
        {
            MessageDispatcher.Publish(new ReactionAddedNotification(message, channel, reaction));

            return Task.CompletedTask;
        }

        private Task OnReactionRemovedAsync(
            Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel,
            SocketReaction reaction)
        {
            MessageDispatcher.Publish(new ReactionRemovedNotification(message, channel, reaction));

            return Task.CompletedTask;
        }

        private Task OnReadyAsync()
        {
            MessageDispatcher.Publish(ReadyNotification.Default);

            return Task.CompletedTask;
        }

        private Task OnRoleCreatedAsync(SocketRole role)
        {
            MessageDispatcher.Publish(new RoleCreatedNotification(role));

            return Task.CompletedTask;
        }

        private Task OnRoleUpdatedAsync(SocketRole oldRole, SocketRole newRole)
        {
            MessageDispatcher.Publish(new RoleUpdatedNotification(oldRole, newRole));

            return Task.CompletedTask;
        }

        private Task OnUserBannedAsync(SocketUser user, SocketGuild guild)
        {
            MessageDispatcher.Publish(new UserBannedNotification(user, guild));

            return Task.CompletedTask;
        }

        private Task OnUserJoinedAsync(SocketGuildUser guildUser)
        {
            MessageDispatcher.Publish(new UserJoinedNotification(guildUser));

            return Task.CompletedTask;
        }

        private Task OnUserLeftAsync(SocketGuildUser guildUser)
        {
            MessageDispatcher.Publish(new UserLeftNotification(guildUser));

            return Task.CompletedTask;
        }
    }
}