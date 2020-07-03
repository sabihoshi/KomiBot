namespace Komi.Services.Interactive.TryParse
{
    public delegate bool TryParseDelegate<T>(string input, out T result);
}