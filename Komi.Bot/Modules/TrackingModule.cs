using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Komi.Bot.Core.TypeReaders;
using Komi.Bot.Services.Interactive;
using Komi.Bot.Services.Tracking;
using Komi.Data;
using Komi.Data.Models.Tracking;
using Komi.Data.Models.Tracking.Scanlation;

namespace Komi.Bot.Modules
{
    [Group("track")]
    [Alias("tracking")]
    [Name("Scanlation Tracking")]
    [Summary("Various commands to track the progress of your scanlation releases.")]
    public class TrackingModule : InteractivePromptBase
    {
        private readonly KomiContext _context;

        public TrackingModule(KomiContext context) => _context = context;

        [Command("new")]
        private async Task NewWorkAsync(string name, WorkArguments workArguments)
        {
            var group = _context.Groups
               .SingleOrDefault(x => x.GroupId == Context.Guild.Id);

            if (group is null)
            {
                await ReplyAsync("You do not have a group! Please make one by doing `group`");
                return;
            }

            if (group.Members.All(x => x.Id != Context.User.Id))
            {
                // User is not part of group error
                return;
            }

            var work = new Work
            {
                Name = name,
                Description = workArguments?.Description,
                Volume = workArguments?.Volume,
                Chapter = workArguments?.Chapter,
                Group = group,
                OverridenStatus = null,
                Jobs = group.GroupSettings.DefaultWorkTypes
                   .Select(x => new Job(x.WorkType))
                   .ToList()
            };

            var emotes = TrackingEmotes.Emotes
               .OrderBy(x => x.Type)
               .Select(x => x.Emotes[WorkStatusEmote.StatusColor.Grey]);

            var embed = new EmbedBuilder()
               .WithTitle(work.Name)
               .WithDescription(work.Description ?? string.Empty)
               .WithImageUrl(work.Status.GetImage());

            await ReplyAsync(string.Join(" ", emotes.Select(x => x.ToString())), embed: embed.Build());

            await _context.Works.AddAsync(work);
            await _context.SaveChangesAsync();
        }

        public enum WorkOptions
        {
            Name,
            Description,
            Volume,
            Chapter
        }

        [Command("new")]
        [Priority(-1)]
        public async Task NewWorkInteractiveAsync()
        {
            var group = _context.Groups
               .SingleOrDefault(x => x.GroupId == Context.Guild.Id);

            if (group is null)
            {
                await ReplyAsync("You do not have a group! Please make one by doing `group`");
                return;
            }

            //if (group.Members.All(x => x.Id != Context.User.Id))
            //{
            //    // User is not part of group error
            //    return;
            //}

            var answers = await
                this.CreatePromptCollection<WorkOptions>()
                   .WithTimeout(30)
                   .WithPrompt(WorkOptions.Name, "Enter the name of the work")
                   .WithPrompt(WorkOptions.Description, "What is the description of the work?", required: false)
                   .WithPrompt(WorkOptions.Volume, "Volume of the work", required: false)
                   .ThatHas<int>(int.TryParse)
                   .WithPrompt(WorkOptions.Chapter, "Chapter of the work")
                   .ThatHas<int>(int.TryParse)
                   .GetAnswersAsync();

            var work = new Work
            {
                Name = answers[WorkOptions.Name],
                Description = answers.GetValueOrDefault(WorkOptions.Description, null)!,
                Volume = answers.GetValueOrDefault(WorkOptions.Volume, null)?.As<int>(),
                Chapter = answers.GetValueOrDefault(WorkOptions.Chapter, null)?.As<int>(),
                Group = group,
                OverridenStatus = null,
                Jobs = group.GroupSettings.DefaultWorkTypes
                   .Select(x => new Job(x.WorkType))
                   .ToList()
            };

            var emotes = TrackingEmotes.Emotes
               .Where(x => group.GroupSettings.DefaultWorkTypes.Select(s => s.WorkType).Contains(x.Type))
               .OrderBy(x => x.Type)
               .Select(x => x.Emotes[WorkStatusEmote.StatusColor.Grey]);

            var embed = new EmbedBuilder()
               .WithTitle(work.Name)
               .WithDescription(work.Description ?? string.Empty)
               .WithImageUrl(work.Status.GetImage());

            var channel = Context.Guild.TextChannels.SingleOrDefault(x => x.Id == group.GroupSettings.TrackingChannel);

            if (channel is null)
            {
                await ReplyAsync("There is no configured tracking channel");
                return;
            }

            await channel.SendMessageAsync(string.Join(" ", emotes.Select(x => x.ToString())), embed: embed.Build());

            await _context.Works.AddAsync(work);
            await _context.SaveChangesAsync();
        }
    }
}