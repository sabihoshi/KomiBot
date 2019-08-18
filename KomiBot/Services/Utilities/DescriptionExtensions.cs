using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Discord.Commands;
using JetBrains.Annotations;
using KomiBot.TypeReaders;

namespace KomiBot.Services.Utilities
{
    public static class DescriptionExtensions
    {
        public static string? GetDescription(this PropertyInfo property)
        {
            try
            {
                return property.GetCustomAttribute<DescriptionAttribute>()?.Description;
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
                return e.GetAttributeOfType<DescriptionAttribute>()?.Description;
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