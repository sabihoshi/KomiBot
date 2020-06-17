using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Komi.Bot.Services.Image;
using Komi.Bot.Services.Interactive.Criteria;
using Komi.Bot.Services.Utilities;
using Microsoft.Extensions.Logging;

namespace Komi.Bot.Services.Interactive
{
    public class InteractivePromptBase : InteractiveBase<SocketCommandContext>
    {
        public IImageService ImageService { get; set; }

        public ILogger<InteractivePromptBase> Logger { get; set; }

        internal async Task<(SocketMessage? response, IUserMessage message)> Prompt(string question,
            IUserMessage? message = null, IEnumerable<EmbedFieldBuilder>? fields = null, int secondsTimeout = 30,
            CriteriaCriterion<SocketMessage>? criterion = null)
        {
            message = await ModifyOrSendMessage(question, message, fields);

            SocketMessage response;
            var timeout = TimeSpan.FromSeconds(secondsTimeout);
            if (criterion is null)
                response = await NextMessageAsync(timeout: timeout);
            else
                response = await NextMessageAsync(timeout: timeout, criterion: criterion);

            _ = response?.DeleteAsync();

            return (response, message);
        }

        internal async Task<IUserMessage> ModifyOrSendMessage(
            string question, IUserMessage? message = null, IEnumerable<EmbedFieldBuilder>? fields = null,
            Color? color = null)
        {
            var embed = new EmbedBuilder()
               .WithUserAsAuthor(Context.User)
               .WithDescription(question)
               .WithColor(color
                       ?? await ImageService.GetDominantColorAsync(new Uri(Context.User.GetDefiniteAvatarUrl())))
               .WithFields(fields ?? Enumerable.Empty<EmbedFieldBuilder>());

            if (message == null)
                return await ReplyAsync(embed: embed.Build());

            await message.ModifyAsync(msg => msg.Embed = embed.Build());
            return message;
        }
    }
}