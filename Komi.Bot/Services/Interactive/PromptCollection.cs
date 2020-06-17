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
        public PromptCollection(InteractivePromptBase context,
            string? errorMessage = null, IServiceProvider? services = null)
        {
            ErrorMessage = errorMessage;
            Context = context;
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

        public InteractivePromptBase Context { get; }
    }

    public partial class PromptOrCollection<T> where T : notnull
    {
        public PromptOrCollection(Prompt<T> prompt, PromptCollection<T> collection)
        {
            Prompt = prompt;
            Collection = collection;
        }

        public Prompt<T> Prompt { get; }

        public PromptCollection<T> Collection { get; }
    }
}