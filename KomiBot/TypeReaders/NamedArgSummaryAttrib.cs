using System;
using Discord.Commands;

namespace KomiBot.TypeReaders
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NamedArgSummaryAttrib : SummaryAttribute
    {
        public NamedArgSummaryAttrib(string text) : base(text) { }
    }
}