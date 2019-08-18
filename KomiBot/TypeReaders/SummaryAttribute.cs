using System;

namespace KomiBot.TypeReaders
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Field)]
    public class SummaryAttribute : Discord.Commands.SummaryAttribute
    {
        public SummaryAttribute(string text) : base(text) { }
    }
}