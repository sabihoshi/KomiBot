using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using JetBrains.Annotations;
using KomiBot.Services.Help;
using KomiBot.Services.Image;
using KomiBot.Services.Utilities;

namespace KomiBot.Modules
{
    [Group("help")]
    [Summary("Provides commands for helping users to understand how to interact with MODiX.")]
    public sealed class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly ICommandHelpService _commandHelpService;
        private readonly IImageService _imageService;

        public HelpModule(ICommandHelpService commandHelpService, IImageService imageService)
        {
            _commandHelpService = commandHelpService;
            _imageService = imageService;
        }

        [Command]
        [Summary("Prints a neat list of all commands.")]
        [UsedImplicitly]
        public async Task HelpAsync()
        {
            var modules = _commandHelpService.GetModuleHelpData().Select(d => d.Name).OrderBy(d => d);

            var descriptionBuilder = new StringBuilder()
               .AppendLine("Modules:")
               .AppendJoin(", ", modules)
               .AppendLine()
               .AppendLine()
               .AppendLine("Do \"k!help dm\" to have everything DMed to you. (Spammy!)")
               .AppendLine("Do \"k!help [module name] to have that module's commands listed.");

            var embed = new EmbedBuilder().WithTitle("Help")
               .WithColor(await GetAvatarColor(Context.User))
               .WithDescription(descriptionBuilder.ToString());

            await ReplyAsync(embed: embed.Build());
        }

        [Command("dm")]
        [Summary("Spams the user's DMs with a list of every command available.")]
        [UsedImplicitly]
        public async Task HelpDMAsync()
        {
            var userDM = await Context.User.GetOrCreateDMChannelAsync();

            foreach (var module in _commandHelpService.GetModuleHelpData().OrderBy(x => x.Name))
            {
                var embed = GetEmbedForModule(module).WithColor(await GetAvatarColor(Context.User));

                try { await userDM.SendMessageAsync(embed: embed.Build()); }
                catch (HttpException ex) when (ex.DiscordCode == 50007)
                {
                    await ReplyAsync(
                        $"You have private messages for this server disabled, {Context.User.Mention}. Please enable them so that I can send you help.");
                    return;
                }
            }

            await ReplyAsync($"Check your private messages, {Context.User.Mention}.");
        }

        [Command]
        [Summary("Retrieves help from a specific module or command.")]
        [Priority(-10)]
        [UsedImplicitly]
        public async Task HelpAsync(
            [Remainder] [Summary("Name of the module or command to query.")]
            string query)
        {
            await HelpAsync(query, HelpDataType.Command | HelpDataType.Module);
        }

        [Command("module")]
        [Alias("modules")]
        [Summary("Retrieves help from a specific module. Useful for modules that have an overlapping command name.")]
        [UsedImplicitly]
        public async Task HelpModuleAsync(
            [Remainder] [Summary("Name of the module to query.")]
            string query)
        {
            await HelpAsync(query, HelpDataType.Module);
        }

        [Command("command")]
        [Alias("commands")]
        [Summary("Retrieves help from a specific command. Useful for commands that have an overlapping module name.")]
        [UsedImplicitly]
        public async Task HelpCommandAsync(
            [Remainder] [Summary("Name of the module to query.")]
            string query)
        {
            await HelpAsync(query, HelpDataType.Command);
        }

        private async Task HelpAsync(string query, HelpDataType type)
        {
            string sanitizedQuery = FormatUtilities.SanitizeAllMentions(query);

            if (TryGetEmbed(query, type, out var embed))
            {
                embed.WithColor(await GetAvatarColor(Context.User));
                await ReplyAsync($"Results for \"{sanitizedQuery}\":", embed: embed.Build());
                return;
            }

            await ReplyAsync($"Sorry, I couldn't find help related to \"{sanitizedQuery}\".");
        }

        private bool TryGetEmbed(
            string query,
            HelpDataType queries,
            [MaybeNullWhen(false)] out EmbedBuilder embed)
        {
            embed = null;

            if (queries.HasFlag(HelpDataType.Command))
            {
                var byCommand = _commandHelpService.GetCommandHelpData(query);
                if (byCommand != null)
                {
                    embed = GetEmbedForCommand(byCommand);
                    return true;
                }
            }

            if (queries.HasFlag(HelpDataType.Module))
            {
                var byModule = _commandHelpService.GetModuleHelpData(query);
                if (byModule != null)
                {
                    embed = GetEmbedForModule(byModule);
                    return true;
                }
            }

            return false;
        }

        [Flags]
        private enum HelpDataType
        {
            Command = 1 << 1,
            Module = 1 << 2
        }

        private EmbedBuilder GetEmbedForModule(ModuleHelpData module)
        {
            var embedBuilder = new EmbedBuilder()
               .WithTitle($"Module: {module.Name}")
               .WithDescription(module.Summary);

            foreach (var command in module.Commands)
                AddCommandFields(embedBuilder, command);

            return embedBuilder;
        }

        private EmbedBuilder GetEmbedForCommand(CommandHelpData command) =>
            AddCommandFields(new EmbedBuilder(), command);

        private EmbedBuilder AddCommandFields(EmbedBuilder embedBuilder, CommandHelpData command)
        {
            var summaryBuilder = new StringBuilder(command.Summary ?? "No summary.").AppendLine();

            string name = command.Aliases.FirstOrDefault();
            AppendAliases(
                summaryBuilder,
                command.Aliases.Where(c => !c.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList());
            AppendParameters(summaryBuilder, command.Parameters);

            embedBuilder.AddField(
                new EmbedFieldBuilder().WithName($"Command: `k!{name} {GetParams(command)}`")
                   .WithValue(summaryBuilder.ToString()));

            return embedBuilder;
        }

        private StringBuilder AppendAliases(StringBuilder stringBuilder, IReadOnlyCollection<string> aliases)
        {
            if (aliases.Count == 0)
                return stringBuilder;

            stringBuilder.AppendLine(Format.Bold("Aliases:"));

            foreach (string alias in FormatUtilities.CollapsePlurals(aliases))
                stringBuilder.AppendLine($"• {alias}");

            return stringBuilder;
        }

        private StringBuilder AppendParameters(
            StringBuilder stringBuilder,
            IReadOnlyCollection<ParameterHelpData>? parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return stringBuilder;

            stringBuilder.AppendLine(Format.Bold("Parameters:"));

            foreach (var parameter in parameters)
            {
                AppendSummary(stringBuilder, parameter);

                if (parameter.Options == null)
                    continue;

                foreach (var option in parameter.Options)
                    AppendSummary(stringBuilder, option);
            }

            return stringBuilder;

            void AppendSummary(StringBuilder sb, ParameterHelpData parameter)
            {
                if (string.IsNullOrEmpty(parameter.Summary))
                    return;

                sb.AppendLine($"• {Format.Bold(parameter.Name)}: {parameter.Summary}");
            }
        }

        private ValueTask<Color> GetAvatarColor(IUser contextUser)
        {
            ValueTask<Color> colorTask = default;

            if ((contextUser.GetAvatarUrl(size: 16) ?? contextUser.GetDefaultAvatarUrl()) is { } avatarUrl)
                colorTask = _imageService.GetDominantColorAsync(new Uri(avatarUrl));

            return colorTask;
        }

        private string GetParams(CommandHelpData info)
        {
            var sb = new StringBuilder();

            var parameterInfo = info.Parameters.Select(GetParamName);
            sb.Append(string.Join(" ", parameterInfo));

            return sb.ToString();
        }

        private string GetParamName(ParameterHelpData parameter)
        {
            if (parameter.Options != null)
            {
                var parameters = parameter.Options.Select(p => p.Name);
                return string.Join("|", parameters).SurroundNullability(parameter.IsOptional);
            }

            return parameter.Name.SurroundNullability(parameter.IsOptional);
        }
    }
}