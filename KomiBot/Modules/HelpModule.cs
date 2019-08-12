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
        [Summary("Prints a neat list of all commands in the supplied module.")]
        [Priority(-10)]
        public async Task HelpAsync([Remainder]string moduleName)
        {
            var foundModule = _commandHelpService.GetModuleHelpData().FirstOrDefault(d => d.Name.IndexOf(moduleName, StringComparison.OrdinalIgnoreCase) >= 0);

            if (foundModule is null)
            {
                await ReplyAsync($"Sorry, I couldn't find the \"{moduleName}\" module.");
                return;
            }

            var embed = GetEmbedForModule(foundModule);

            await ReplyAsync($"Results for \"{moduleName}\":", embed: embed.Build());

        }

        private EmbedBuilder GetEmbedForModule(ModuleHelpData module)
        {
            var embedBuilder = new EmbedBuilder()
                              .WithTitle($"Module: {module.Name}")
                              .WithDescription(module.Summary);

            return AddCommandFields(embedBuilder, module.Commands);
        }

        private EmbedBuilder AddCommandFields(EmbedBuilder embedBuilder, IEnumerable<CommandHelpData> commands)
        {
            foreach (var command in commands)
            {
                var summaryBuilder = new StringBuilder(command.Summary ?? "No summary.").AppendLine();
                var summary = AppendAliases(summaryBuilder, command.Aliases);

                embedBuilder.AddField(new EmbedFieldBuilder()
                                     .WithName($"Command: !{command.Aliases.FirstOrDefault()} {GetParams(command)}")
                                     .WithValue(summary.ToString()));
            }

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

        private string GetParams(CommandHelpData info)
        {
            var sb = new StringBuilder();

            foreach (var parameter in info.Parameters)
            {
                if (parameter.IsOptional)
                    sb.Append($"[Optional({parameter.Name})]");
                else
                    sb.Append($"[{parameter.Name}]");
            }

            return sb.ToString();
        }
    }
}