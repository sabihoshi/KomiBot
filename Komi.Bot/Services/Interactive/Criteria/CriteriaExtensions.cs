using System;
using System.Collections.Generic;
using Discord.Addons.Interactive;
using Discord.Commands;

namespace Komi.Bot.Services.Interactive.Criteria
{
    public static class CriteriaExtensions
    {
        public static IEnumerable<ICriterion<T>> GetCriteria<T>(this IPromptCriteria<T> promptCriteria)
        {
            var criteria = new List<ICriterion<T>>();

            if (promptCriteria.Criteria != null)
                criteria.AddRange(promptCriteria.Criteria);

            if (promptCriteria.TypeReader != null)
                criteria.Add((ICriterion<T>)promptCriteria.TypeReader.AsCriterion());

            return criteria;
        }

        public static TypeReaderCriterion AsCriterion(this TypeReader reader, IServiceProvider? services = null) =>
            new TypeReaderCriterion(reader, services);

        public static CriteriaCriterion<T> AsCriterion<T>(this IEnumerable<ICriterion<T>> criteria) =>
            new CriteriaCriterion<T>(criteria);
    }
}