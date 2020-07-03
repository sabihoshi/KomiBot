using System;

namespace Komi.Services.Core
{
    public class FunModuleService : IFunModuleService
    {
        private readonly string[] _hugEmotes =
        {
            "ヾ(≧▽≦*)o", "q(≧▽≦q)", "(～￣▽￣)～", "( •̀ ω •́ )✧", "~(￣▽￣)~",
            "*φ(゜▽゜*)♪", "o(*^＠^*)o", "(✿◡‿◡)`", "(*>﹏<*)′", "(*^▽^*)",
            "(≧∇≦)ﾉ", "(´▽`ʃ♡ƪ)", "(●ˇ∀ˇ●)○", "( ＾皿＾)っ", "(￣y▽￣)",
            "(*°▽°*)╯", "ヾ(•ω•`)o", "( ´･･)ﾉ(._.`)", "(づ￣ 3￣)づ（＾∀＾●）ﾉ",
            "ｼヾ(^▽^*)))", "\\(@^0^@)/", "（づ￣3￣）づ╭❤～"
        };

        private readonly Random _random = new Random();

        public string RandomEmote() => _hugEmotes[_random.Next(_hugEmotes.Length)];
    }
}