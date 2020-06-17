using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using Discord.Commands;

namespace Komi.Bot.Services.Interactive.Criteria
{
    public class CriteriaCriterion<T> : ICriterion<T>
    {
        private readonly IEnumerable<ICriterion<T>> _criteria;

        public CriteriaCriterion(IEnumerable<ICriterion<T>> criteria) => _criteria = criteria;

        public CriteriaCriterion(params ICriterion<T>[] criteria) => _criteria = criteria;

        public async Task<bool> JudgeAsync(SocketCommandContext sourceContext, T parameter)
        {
            var judges = _criteria
               .Select(c => c.JudgeAsync(sourceContext, parameter));

            var results = await Task.WhenAll(judges);

            return results.All(r => r);
        }
    }
}