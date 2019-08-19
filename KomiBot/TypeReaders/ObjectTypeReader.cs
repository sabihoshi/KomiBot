using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace KomiBot.TypeReaders
{
    public class ObjectTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            object result = input;
            return Task.FromResult(TypeReaderResult.FromSuccess(result));
        }
    }
}
