using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Humanizer;
using Komi.Bot.Services.Interactive;
using Komi.Bot.Services.Interactive.TypeReaders;
using Komi.Bot.Services.Tracking;
using Komi.Data.Models.Tracking;
using Serilog;

namespace Komi.Bot.Modules
{
    [Group("group")]
    [Summary("Various commands in order to manage your scanlation group.")]
    public class GroupModule : InteractivePromptBase
    {
        private enum GroupOptions
        {
            Name,
            WorkType,
            TrackingChannel
        }

        private readonly GroupService _groupService;

        public GroupModule(GroupService groupService) => _groupService = groupService;

        [Command("create")]
        [Summary("Create a new group. Each server can only have one group each.")]
        public async Task CreateGroupAsync()
        {
            if (_groupService.GroupExists(Context.Guild.Id))
            {
                await ReplyAsync("This server already has a group!");

                // return;
            }

            var workTypeFields = new[]
            {
                new EmbedFieldBuilder()
                   .WithName("Possible types")
                   .WithValue(Enum.GetValues(typeof(WorkType))
                       .Cast<WorkType>()
                       .Skip(1)
                       .Select(w => w.Humanize())
                       .Humanize())
                   .WithIsInline(true),

                new EmbedFieldBuilder()
                   .WithName("Default values")
                   .WithValue(WorkTypeExtensions.Default.Select(
                            w => w.Humanize())
                       .Humanize())
                   .WithIsInline(true)
            };

            try
            {
                var answers = await this.CreatePromptCollection<GroupOptions>()
                   .WithTimeout(30)
                   .WithPrompt(GroupOptions.Name, "Enter the name of your group")
                   .WithPrompt(GroupOptions.WorkType,
                        "Enter the default job types whenever you create a new scanlation work.\n"
                      + "Their abbreviations will work as well (TL, QC, PR, etc.)", workTypeFields, false, 120)
                   .WithPrompt(GroupOptions.TrackingChannel,
                        "Enter the channel in which you things will be tracked on.")
                       .ThatHas(Reader.Channel<ITextChannel>())
                   .GetAnswersAsync();

                await _groupService.CreateGroupAsync(
                    Context.Guild.Id,
                    answers.Get<string>(GroupOptions.Name),
                    answers.Get<string>(GroupOptions.WorkType).GetWorkTypes(),
                    answers.Get<ITextChannel>(GroupOptions.TrackingChannel).Id);

                Log.Information("A new group was added. Info: {0}", answers);
            }
            catch (Exception e) { Log.Error(e.Message); }
        }
    }
}