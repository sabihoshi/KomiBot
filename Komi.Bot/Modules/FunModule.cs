using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Komi.Bot.Services.Core;

namespace Komi.Bot.Modules
{
    public class FunModule : ModuleBase<SocketCommandContext>
    {
        private FunModuleService FunModuleService { get; set; }

        [Command("hug")]
        [Summary("Hug a user or Komi!")]
        [RequireContext(ContextType.Guild)]
        public Task HugAsync([Optional, Summary("The user to hug")] IGuildUser user)
        {
            if (user is null)
                return ReplyAsync("You hugged the air, silly.");

            return user.Id == Context.Client.CurrentUser.Id 
                ? ReplyAsync($"You hugged me! Komi hugs you back~")
                : ReplyAsync($"You hugged {user.Username} {FunModuleService.RandomEmote()}");
        }

        [Command("pat")]
        [Alias("headpat")]
        [Summary("Pat a user or Komi!")]
        [RequireContext(ContextType.Guild)]
        public Task PatAsync([Optional, Summary("The user to pat")] IGuildUser user)
        {
            if (user is null)
                return ReplyAsync("You patted the air, silly.");

            return user.Id == Context.Client.CurrentUser.Id 
                ? ReplyAsync($"You patted me! Komi pats you back~")
                : ReplyAsync($"You patted {user.Username} {FunModuleService.RandomEmote()}");
        }
    }
}
