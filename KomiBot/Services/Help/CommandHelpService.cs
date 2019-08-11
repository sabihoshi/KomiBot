﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Discord.Commands;

namespace KomiBot.Services.Help
{
    /// <summary>
    ///     Provides functionality to retrieve command help information.
    /// </summary>
    public interface ICommandHelpService
    {
        /// <summary>
        ///     Retrieves help data for all available modules.
        /// </summary>
        /// <returns>
        ///     A readonly collection of data about all available modules.
        /// </returns>
        IReadOnlyCollection<ModuleHelpData> GetModuleHelpData();
    }

    /// <inheritdoc />
    public class CommandHelpService : ICommandHelpService
    {
        private readonly CommandService _commandService;
        private IReadOnlyCollection<ModuleHelpData> _cachedHelpData;

        public CommandHelpService(CommandService commandService)
        {
            _commandService = commandService;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<ModuleHelpData> GetModuleHelpData()
        {
            return LazyInitializer.EnsureInitialized(ref _cachedHelpData, () =>
                _commandService.Modules
                               .Where(x => !x.Attributes.Any(attr => attr is HiddenFromHelpAttribute))
                               .Select(x => ModuleHelpData.FromModuleInfo(x))
                               .ToArray());
        }
    }
}