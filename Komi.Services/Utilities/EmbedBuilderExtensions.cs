using Discord;

namespace Komi.Bot.Services.Utilities
{
    public static class EmbedBuilderExtensions
    {
        public static EmbedBuilder WithUserAsAuthor(this EmbedBuilder builder, IUser user, bool withNickname = true) =>
            builder
               .WithAuthor(user.GetFullUsername(),
                    user.GetDefiniteAvatarUrl());

        public static EmbedBuilder WithUserAsAuthor(this EmbedBuilder builder, IGuildUser user,
            bool withNickname = true)
        {
            var nickname = withNickname ? $" ({user.Nickname})" : string.Empty;
            return builder
               .WithAuthor(user.GetFullUsername() + withNickname,
                    user.GetDefiniteAvatarUrl());
        }
    }
}