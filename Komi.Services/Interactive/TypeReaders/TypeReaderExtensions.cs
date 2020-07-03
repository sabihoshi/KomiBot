using System;
using Discord.Commands;
using Komi.Services.Interactive.TryParse;

namespace Komi.Services.Interactive.TypeReaders
{
    public static class TypeReaderExtensions
    {
        public static TryParseTypeReader<T> AsTypeReader<T>(this TryParseDelegate<T> tryParse) =>
            new TryParseTypeReader<T>(tryParse);

        public static TypeReaderCriterion AsCriterion(this TypeReader reader, IServiceProvider? services = null) =>
            new TypeReaderCriterion(reader, services);
    }
}