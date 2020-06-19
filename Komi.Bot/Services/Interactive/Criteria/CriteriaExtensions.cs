using System;
using System.Collections.Generic;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Komi.Bot.Services.Interactive.TryParse;
using Komi.Bot.Services.Interactive.TypeReaders;

namespace Komi.Bot.Services.Interactive.Criteria
{
    public static class CriteriaExtensions
    {
        public static IEnumerable<ICriterion<T>> GetCriteria<T>(this IPromptCriteria<T> promptCriteria)
            where T : SocketMessage
        {
            var criteria = new List<ICriterion<T>>();

            if (promptCriteria.Criteria != null)
                criteria.AddRange(promptCriteria.Criteria);

            if (promptCriteria.TypeReader != null)
                criteria.Add(promptCriteria.TypeReader.AsCriterion<T>());

            return criteria;
        }

        public static ICriterion<T> AsCriterion<T>(this TypeReader reader, IServiceProvider? services = null)
            where T : SocketMessage =>
            (ICriterion<T>)reader.AsCriterion(services);

        public static CriteriaCriterion<T> AsCriterion<T>(this IEnumerable<ICriterion<T>> criteria) =>
            new CriteriaCriterion<T>(criteria);

        public static CriteriaCriterion<T> AsCriterion<T>(this ICriterion<T> criteria) =>
            new CriteriaCriterion<T>(criteria);

        public static TryParseCriterion<T> AsCriterion<T>(this TryParseDelegate<T> tryParse) =>
            new TryParseCriterion<T>(tryParse);
    }
}