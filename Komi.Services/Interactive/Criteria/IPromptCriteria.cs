using System.Collections.Generic;
using Discord.Addons.Interactive;
using Discord.Commands;

namespace Komi.Services.Interactive.Criteria
{
    public interface IPromptCriteria<T>
    {
        TypeReader? TypeReader { get; set; }

        ICollection<ICriterion<T>>? Criteria { get; }
    }
}