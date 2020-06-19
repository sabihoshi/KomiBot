using System;
using System.Collections.Generic;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Komi.Bot.Services.Interactive.Criteria;

namespace Komi.Bot.Services.Interactive
{
    public partial class PromptCollection<T> : IPromptCriteria<SocketMessage> where T : notnull
    {
        public PromptCollection(
            InteractivePromptBase module,
            string? errorMessage = null, IServiceProvider? services = null)
        {
            ErrorMessage = errorMessage;
            Module = module;
            Services = services;
            Criteria = new ICriterion<SocketMessage>[]
            {
                new EnsureSourceChannelCriterion(),
                new EnsureSourceUserCriterion()
            };
        }

        public List<Prompt<T>> Prompts { get; } = new List<Prompt<T>>();

        public ICollection<ICriterion<SocketMessage>> Criteria { get; }

        public IServiceProvider? Services { get; }

        public TypeReader? TypeReader { get; set; }

        public int Timeout { get; set; } = 30;

        public string? ErrorMessage { get; set; }

        public InteractivePromptBase Module { get; }

        public SocketCommandContext Context => Module.Context;
    }

    public partial class PromptOrCollection<TOptions>
        where TOptions : notnull
    {
        public PromptOrCollection(Prompt<TOptions> prompt, PromptCollection<TOptions> collection)
        {
            Prompt = prompt;
            Collection = collection;
        }

        public Prompt<TOptions> Prompt { get; }

        public PromptCollection<TOptions> Collection { get; }
    }
}