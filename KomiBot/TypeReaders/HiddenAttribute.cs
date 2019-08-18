using System;

namespace KomiBot.TypeReaders
{
    /// <summary>
    ///     Hides the module or command from display
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HiddenAttribute : Attribute { }
}