using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using KomiBot.Services.Help;
using KomiBot.Services.Utilities;

namespace KomiBot.Modules
{
    [Group("help")]
    [Summary("Provides commands for helping users to understand how to interact with MODiX.")]
    public sealed class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly ICommandHelpService _commandHelpService;

        public HelpModule(ICommandHelpService commandHelpService)
        {
            _commandHelpService = commandHelpService;
        }

        [Command]
        [Summary("Prints a neat list of all commands.")]
        public async Task HelpAsync()
        {
            var modules = _commandHelpService.GetModuleHelpData()
                                             .Select(d => d.Name)
                                             .OrderBy(d => d);

            var descriptionBuilder = new StringBuilder()
                                    .AppendLine("Modules:")
                                    .AppendJoin(", ", modules)
                                    .AppendLine()
                                    .AppendLine()
                                    .AppendLine("Do \"k!help dm\" to have everything DMed to you. (Spammy!)")
                                    .AppendLine("Do \"k!help [module name] to have that module's commands listed.");

            var embed = new EmbedBuilder()
                       .WithTitle("Help")
                       .WithDescription(descriptionBuilder.ToString());

            await ReplyAsync(embed: embed.Build());
        }

        [Command("dm")]
        [Summary("Spams the user's DMs with a list of every command available.")]
        public async Task HelpDMAsync()
        {
            var userDM = await Context.User.GetOrCreateDMChannelAsync();

            foreach (var module in _commandHelpService.GetModuleHelpData().OrderBy(x => x.Name))
            {
                var embed = GetEmbedForModule(module);

                try
                {
                    await userDM.SendMessageAsync(embed: embed.Build());
                }
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
        [Summary("Prints a neat list of all commands based on the supplied query.")]
        [Priority(-10)]
        public async Task HelpAsync(
            [Remainder] [Summary("The module name or related query to use to search for the help module.")]
            string query)
        {
            var foundModule = _commandHelpService.GetModuleHelpData(query);
            var foundCommand = _commandHelpService.GetCommandHelpData(query);

            var sanitizedQuery = FormatUtilities.SanitizeAllMentions(query);

            if (foundModule is null && foundCommand is null)
            {
                await ReplyAsync($"Sorry, I couldn't find help related to \"{sanitizedQuery}\".");
                return;
            }

            var embed = foundModule == null ? GetEmbedForCommand(foundCommand) : GetEmbedForModule(foundModule);

            await ReplyAsync($"Results for \"{sanitizedQuery}\":", embed: embed.Build());
        }

        private EmbedBuilder GetEmbedForModule(ModuleHelpData module)
        {
            var embedBuilder = new EmbedBuilder()
                              .WithTitle($"Module: {module.Name}")
                              .WithDescription(module.Summary);

            foreach (var command in module.Commands) AddCommandFields(embedBuilder, command);

            return embedBuilder;
        }

        private EmbedBuilder GetEmbedForCommand(CommandHelpData command)
        {
            return AddCommandFields(new EmbedBuilder(), command);
        }

        private EmbedBuilder AddCommandFields(EmbedBuilder embedBuilder, CommandHelpData command)
        {
            var summaryBuilder = new StringBuilder(command.Summary ?? "No summary.").AppendLine();

            var name = command.Aliases.FirstOrDefault();
            AppendAliases(summaryBuilder, command.Aliases
                                                 .Where(c => !c.Equals(name,
                                                      StringComparison.OrdinalIgnoreCase))
                                                 .ToList());
            AppendParameters(summaryBuilder, command.Parameters);

            embedBuilder.AddField(new EmbedFieldBuilder()
                                 .WithName($"Command: `k!{name} {GetParams(command)}`")
                                 .WithValue(summaryBuilder.ToString()));

            return embedBuilder;
        }

        private StringBuilder AppendAliases(StringBuilder stringBuilder, IReadOnlyCollection<string> aliases)
        {
            if (aliases.Count == 0)
                return stringBuilder;

            stringBuilder.AppendLine(Format.Bold("Aliases:"));

            foreach (var alias in FormatUtilities.CollapsePlurals(aliases)) stringBuilder.AppendLine($"• {alias}");

            return stringBuilder;
        }

        private StringBuilder AppendParameters(StringBuilder stringBuilder,
            IReadOnlyCollection<ParameterHelpData> parameters)
        {
            if (parameters.Count == 0)
                return stringBuilder;

            stringBuilder.AppendLine(Format.Bold("Parameters:"));

            foreach (var parameter in parameters)
            {
                AppendSummary(stringBuilder, parameter);

                if (parameter.Options == null) continue;

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