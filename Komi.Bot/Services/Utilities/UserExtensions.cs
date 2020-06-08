using System.Linq;
using Discord;

namespace Komi.Bot.Services.Utilities
{
    public static class UserExtensions
    {
        public static string GetFullUsername(this IUser user) => $"{user.Username}#{user.Discriminator}";

        public static bool HasRole(this IGuildUser user, ulong roleId) => user.RoleIds.Contains(roleId);

        public static string GetDefiniteAvatarUrl(this IUser user) => user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl();
    }
}