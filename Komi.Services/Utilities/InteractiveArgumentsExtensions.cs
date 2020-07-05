//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Discord;
//using Discord.Addons.Interactive;
//using Discord.Commands;
//using Discord.WebSocket;

//namespace Komi.Bot.Services.Utilities
//{
//    public class InteractiveArguments : IReactionCallback
//    {
//        public SocketCommandContext Context { get; }

//        public InteractiveService Interactive { get; private set; }

//        public IUserMessage Message { get; private set; }

//        public RunMode RunMode => RunMode.Sync;

//        public ICriterion<SocketReaction> Criterion { get; }

//        public TimeSpan? Timeout => options.Timeout;

//        private readonly PaginatedMessage _pager;

//        private PaginatedAppearanceOptions options => _pager.Options;

//        private readonly int pages;
//        private int page = 1;

//        public async IAsyncEnumerable<SocketMessage> InteractiveArguments<T>(this T results)
//        {
//            foreach (var type in typeof(T).GetProperties())
//            {
//                if (type.GetValue(results) != null)
//                    continue;

//                _ = Task.Run(async () =>
//                {
//                    var criteria = new Criteria<SocketMessage>()
//                       .AddCriterion(new EnsureSourceChannelCriterion())
//                       .AddCriterion(new EnsureFromUserCriterion(reaction.UserId))
//                       .AddCriterion(new EnsureIsIntegerCriterion());
//                    var response = await Interactive.NextMessageAsync(Context, criteria, TimeSpan.FromSeconds(15));
//                    int request = int.Parse(response.Content);
//                    if (request < 1 || request > pages)
//                    {
//                        _ = response.DeleteAsync().ConfigureAwait(false);
//                        await Interactive.ReplyAndDeleteAsync(Context, options.Stop.Name);
//                        return;
//                    }
//                    page = request;
//                    _ = response.DeleteAsync().ConfigureAwait(false);
//                    await RenderAsync().ConfigureAwait(false);
//                });
//            }
//        }

//        public Task<bool> HandleCallbackAsync(SocketReaction reaction) => throw new NotImplementedException();
//    }
//}

