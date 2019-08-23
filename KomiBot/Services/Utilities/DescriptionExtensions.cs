using System;
using System.Reflection;
using JetBrains.Annotations;
using KomiBot.Core.Attributes;

namespace KomiBot.Services.Utilities
{
    public static class DescriptionExtensions
    {
        public static string? GetDescription(this PropertyInfo property)
        {
            try
            {
                return property.GetCustomAttribute<DescriptionAttribute>()?.Text;
            }
            catch
            {
                return null;
            }
        }

        public static string? GetDescription(this Enum e)
        {
            try
            {
                return e.GetAttributeOfType<DescriptionAttribute>()?.Text;
            }
            catch
            {
                return null;
            }
        }

        [CanBeNull]
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            return type.GetField(Enum.GetName(type, enumVal)).GetCustomAttribute<T>();
        }
    }
}