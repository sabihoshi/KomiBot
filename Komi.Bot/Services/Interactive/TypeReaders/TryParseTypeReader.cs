using System;
using System.Threading.Tasks;
using Discord.Commands;
using Komi.Bot.Services.Interactive.TryParse;

namespace Komi.Bot.Services.Interactive.TypeReaders
{
    public class TryParseTypeReader<T> : TypeReader
    {
        private readonly TryParseDelegate<T> _tryParse;

        public TryParseTypeReader(TryParseDelegate<T> tryParse) => _tryParse = tryParse;

        public override async Task<TypeReaderResult> ReadAsync(
            ICommandContext context, string input, IServiceProvider services) =>
            _tryParse(input, out var result)
                ? TypeReaderResult.FromSuccess(result)
                : TypeReaderResult.FromError(CommandError.ParseFailed, "Invalid input");
    }
}