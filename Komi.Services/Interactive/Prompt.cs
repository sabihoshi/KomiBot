using System.Collections.Generic;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Komi.Services.Interactive.Criteria;

namespace Komi.Services.Interactive
{
    public class Prompt<T> : IPromptCriteria<SocketMessage> where T : notnull
    {
        public Prompt(
            T key, string question, IEnumerable<EmbedFieldBuilder>? fields, bool isRequired, int? timeout,
            TypeReader? typeReader = null)
        {
            Key = key;
            Question = question;
            Fields = fields;
            IsRequired = isRequired;
            Timeout = timeout;
            TypeReader = typeReader;
            Criteria = new List<ICriterion<SocketMessage>>();
        }

        public T Key { get; }

        public string Question { get; }

        public IEnumerable<EmbedFieldBuilder>? Fields { get; }

        public bool IsRequired { get; }

        public int? Timeout { get; }

        public TypeReader? TypeReader { get; set; }

        public ICollection<ICriterion<SocketMessage>>? Criteria { get; }
    }
}