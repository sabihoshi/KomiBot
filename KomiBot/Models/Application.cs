using JetBrains.Annotations;

namespace KomiBot.Models
{
    public class Application
    {
        public Application(ulong owner, [NotNull] string token)
        {
            Owner = owner;
            Token = token;
        }

        [NotNull] public ulong Owner { get; }

        [NotNull] public string Token { get; }
    }
}