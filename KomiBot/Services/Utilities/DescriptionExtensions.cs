using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace KomiBot.Services.Utilities
{
    public static class DescriptionExtensions
    {
        public static T? GetAttributeOfEnum<T>(this Enum obj) where T : Attribute
        {
            var type = obj.GetType();
            var name = Enum.GetName(type, obj);
            return name is null ? null : type.GetField(name)?.GetCustomAttribute<T>();
        }
    }
}