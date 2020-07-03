namespace Komi.Services.Core
{
    public interface IFunModuleService
    {
        /// <summary>
        ///     Generate a random emote
        /// </summary>
        /// <returns>A random Kaomoji in string</returns>
        string RandomEmote();
    }
}