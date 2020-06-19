using System;
using Discord.Commands;
using Komi.Bot.Services.Interactive.TryParse;

namespace Komi.Bot.Services.Interactive.TypeReaders
{
    public static class TypeReaderExtensions
    {
        public static TryParseTypeReader<T> AsTypeReader<T>(this TryParseDelegate<T> tryParse) =>
            new TryParseTypeReader<T>(tryParse);

        public static TypeReaderCriterion AsCriterion(this TypeReader reader, IServiceProvider? services = null) =>
            new TypeReaderCriterion(reader, services);
    }
}