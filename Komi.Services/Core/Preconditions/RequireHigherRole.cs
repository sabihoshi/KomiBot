using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Komi.Services.Core.Preconditions
{
    public class RequireHigherRole : ParameterPreconditionAttribute
    {
        private readonly string? _command;

        public RequireHigherRole(string? command = null) => _command = command;

        public override async Task<PreconditionResult> CheckPermissionsAsync(
            ICommandContext context,
            ParameterInfo parameter,
            object value,
            IServiceProvider services)
        {
            // Hierarchy is only available under the socket variant of the user.
            if (!(context.User is SocketGuildUser guildUser))
                return PreconditionResult.FromError("This command cannot be used outside of a guild.");

            var targetUser = value switch
            {
                SocketGuildUser t => t,
                ulong userId      => await context.Guild.GetUserAsync(userId).ConfigureAwait(false) as SocketGuildUser,
                _                 => throw new ArgumentOutOfRangeException()
            };

            if (targetUser == null)
                return PreconditionResult.FromError("User not found.");

            if (guildUser.Hierarchy < targetUser.Hierarchy)
                return PreconditionResult.FromError($"You cannot {_command ?? parameter.Command.Name} this user.");

            var bot = await context.Guild.GetCurrentUserAsync().ConfigureAwait(false) as SocketGuildUser;

            if (targetUser == bot)
                return PreconditionResult.FromError($"You cannot {_command ?? parameter.Command.Name} the bot.");

            if (bot?.Hierarchy < targetUser.Hierarchy)
                return PreconditionResult.FromError("The bot's role is lower than the targeted user.");

            return PreconditionResult.FromSuccess();
        }
    }
}