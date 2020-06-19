using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Komi.Bot.Services.Interactive.TypeReaders
{
    public static class Optional
    {
        public const string SkipString = "skip";

        public static bool IsSkipped(this string message) =>
            message.Equals(SkipString, StringComparison.OrdinalIgnoreCase);

        public static bool IsSkipped(this SocketMessage? message) =>
            message?.Content.Equals(SkipString, StringComparison.OrdinalIgnoreCase) ?? true;
    }

    public class OptionalTypeReader : TypeReader
    {
        private readonly TypeReader _reader;
        private readonly IServiceProvider? _services;

        public OptionalTypeReader(TypeReader reader, IServiceProvider? services = null)
        {
            _reader = reader;
            _services = services;
        }

        public override async Task<TypeReaderResult> ReadAsync(
            ICommandContext context, string input, IServiceProvider services)
        {
            var result = await _reader.ReadAsync(context, input, services);

            return result.IsSuccess || input.IsSkipped()
                ? TypeReaderResult.FromSuccess(result)
                : TypeReaderResult.FromError(result);
        }
    }
}