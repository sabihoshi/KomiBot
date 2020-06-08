using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Komi.Data.Models.Tracking
{
    public enum WorkType
    {
        None = 1 << 0,
        RawProvider = 1 << 1,
        Translator = 1 << 2,
        Proofreading = 1 << 3,
        Cleaning = 1 << 4,
        Redrawing = 1 << 5,
        Typesetter = 1 << 6,
        QualityChecker = 1 << 7,
        Uploader = 1 << 8
    }

    public static class WorkTypeExtensions
    {
        private const RegexOptions DefaultOptions =
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

        private static readonly IReadOnlyDictionary<WorkType, Regex> WorkTypePatterns = new Dictionary<WorkType, Regex>
        {
            [WorkType.RawProvider] = new Regex(@"\bR(aws?|(aws?\s*)?P(roviders?)?)\b", DefaultOptions),
            [WorkType.Translator] = new Regex(@"\bTL|Translat((ion|or|e)s?)\b", DefaultOptions),
            [WorkType.Proofreading] = new Regex(@"\bPR|Proofread(ing|er)?s?\b", DefaultOptions),
            [WorkType.Cleaning] = new Regex(@"\bCL|Clean(ing|er)?s?\b", DefaultOptions),
            [WorkType.Redrawing] = new Regex(@"\bRD|Redraw((ing|er)?s?)?\b", DefaultOptions),
            [WorkType.Typesetter] = new Regex(@"\bTS|Typeset((t(er|ing))?s?)?\b", DefaultOptions),
            [WorkType.QualityChecker] = new Regex(@"\bQ(uality\s*)Check(ing|er)?s?\b", DefaultOptions),
            [WorkType.Uploader] = new Regex(@"\bUP(loads?)\b", DefaultOptions)
        };

        private static IEnumerable<WorkType> GetWorkTypes(this string input)
        {
            return WorkTypePatterns
               .Where(pair => pair.Value.IsMatch(input))
               .Select(pair => pair.Key);
        }
    }
}