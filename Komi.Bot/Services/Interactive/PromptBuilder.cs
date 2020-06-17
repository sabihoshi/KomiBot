using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Komi.Bot.Services.Interactive.Criteria;

namespace Komi.Bot.Services.Interactive
{
    public partial class PromptCollection<T> where T : notnull
    {
        public PromptCollection<T> WithCriterion(ICriterion<SocketMessage> criterion) =>
            this.Modify(c => c.Criteria?.Add(criterion));

        public PromptCollection<T> WithTypeReader(TypeReader reader) => this.Modify(p => p.TypeReader = reader);

        public PromptCollection<T> WithTimeout(int timeout) => this.Modify(c => c.Timeout = timeout);

        public PromptCollection<T> WithError(string error) => this.Modify(c => c.ErrorMessage = error);

        public PromptOrCollection<T> WithPrompt(T key, string question, IEnumerable<EmbedFieldBuilder>? fields = null,
            bool required = true, int? timeOut = null)
        {
            var prompt = new Prompt<T>(key, question, fields, required, timeOut);
            this.Modify(c => c.Prompts.Add(prompt));
            return new PromptOrCollection<T>(prompt, this);
        }

        public async Task<Dictionary<T, PromptResult>> GetAnswersAsync(bool deleteResponse = true)
        {
            var ret = new Dictionary<T, PromptResult>();
            IUserMessage? message = null;
            foreach (var prompt in Prompts)
            {
                var criteria =
                    this.GetCriteria()
                       .Concat(prompt.GetCriteria())
                       .OfType<ICriterion<SocketMessage>>()
                       .AsCriterion();

                var result = await Context.Prompt(prompt.Question, deleteResponse ? message : null, prompt.Fields,
                    prompt.Timeout ?? Timeout,
                    criteria);
                message = result.message;

                if (result.response is null)
                {
                    if (!prompt.IsRequired)
                        continue;

                    if (ErrorMessage != null)
                    {
                        await Context.ModifyOrSendMessage(ErrorMessage ?? "You did not respond in time.", message,
                            color: Color.Red);
                    }
                    throw new ArgumentNullException(nameof(result.response), "User did not respond in time");
                }

                var promptResult = new PromptResult(prompt.Question, result.response.Content);
                ret[prompt.Key] = promptResult;
            }
            return ret;
        }
    }

    public partial class PromptOrCollection<T> where T : notnull
    {
        public PromptCollection<T> ThatHasTypeReader(TypeReader reader)
        {
            Prompt.Modify(p => p.TypeReader = reader);
            return Collection;
        }

        public PromptCollection<T> ThatHasCriterion(ICriterion<SocketMessage> criterion)
        {
            Prompt.Modify(p => p.Criteria?.Add(criterion));
            return Collection;
        }

        public PromptOrCollection<T> WithPrompt(T key, string question, IEnumerable<EmbedFieldBuilder>? fields = null,
            bool required = true, int? timeOut = null)
        {
            var prompt = new Prompt<T>(key, question, fields, required, timeOut);
            var collection = Collection.Modify(c => c.Prompts.Add(prompt));
            return new PromptOrCollection<T>(prompt, collection);
        }
    }

    public static class PromptBuilderExtensions
    {
        public static PromptCollection<T> CreatePromptCollection<T>(this InteractivePromptBase context,
            string? errorMessage = null) where T : notnull =>
            new PromptCollection<T>(context, errorMessage);

        public static T Modify<T>(this T t, Action<T> action)
        {
            action(t);
            return t;
        }
    }
}