using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Commands;
using Humanizer;
using KomiBot.Core.Attributes;

namespace KomiBot.Services.Help
{
    public class ModuleHelpData
    {
        public string Name { get; set; }

        public string? Summary { get; set; }

        public IReadOnlyCollection<CommandHelpData> Commands { get; set; }

        public IReadOnlyCollection<string> HelpTags { get; set; }

        public static ModuleHelpData FromModuleInfo(ModuleInfo module)
        {
            string moduleName = module.Name;

            int suffixPosition = moduleName.IndexOf("Module", StringComparison.Ordinal);
            if (suffixPosition > -1)
                moduleName = module.Name.Substring(0, suffixPosition).Humanize();

            moduleName = moduleName.ApplyCase(LetterCasing.Title);

            var ret = new ModuleHelpData
            {
                Name = moduleName, Summary = string.IsNullOrWhiteSpace(module.Summary) ? "No Summary" : module.Summary,
                Commands = module.Commands.Where(x => !ShouldBeHidden(x))
                   .Select(x => CommandHelpData.FromCommandInfo(x))
                   .ToArray(),
                HelpTags = module.Attributes.OfType<HelpTagsAttribute>().SingleOrDefault()?.Tags
                        ?? Array.Empty<string>()
            };

            return ret;

            bool ShouldBeHidden(CommandInfo command)
            {
                return command.Preconditions.Any(x => x is RequireOwnerAttribute)
                    || command.Attributes.Any(x => x is HiddenAttribute);
            }
        }
    }
}