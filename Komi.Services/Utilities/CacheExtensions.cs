using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Komi.Bot.Core.Attributes;

namespace Komi.Bot.Services.Utilities
{
    public static class CacheExtensions
    {
        private static readonly Cache<Type, IReadOnlyCollection<PropertyInfo>> CachedPrimitives =
            new Cache<Type, IReadOnlyCollection<PropertyInfo>>(GetPrimitives);

        private static readonly Cache<Type, IReadOnlyCollection<PropertyInfo>> CachedLists =
            new Cache<Type, IReadOnlyCollection<PropertyInfo>>(GetLists);

        private static readonly Cache<Type, IReadOnlyCollection<PropertyInfo>> CachedProperties =
            new Cache<Type, IReadOnlyCollection<PropertyInfo>>(GetProperties);

        private static readonly Cache<PropertyInfo, Type> TypeCache = new Cache<PropertyInfo, Type>(GetType);

        private static readonly Cache<(MemberInfo, Type), Attribute?> AttributeCache =
            new Cache<(MemberInfo Member, Type Type), Attribute?>(GetAttributeFromMember);

        private static readonly Cache<(Enum, Type), Attribute?> EnumAttributeCache =
            new Cache<(Enum, Type), Attribute?>(GetAttributeFromEnum);

        private static IReadOnlyCollection<PropertyInfo> GetPrimitives(Type t)
        {
            return CachedProperties[t]
               .Where(p => !p.PropertyType.IsGenericType)
               .ToArray();
        }

        private static IReadOnlyCollection<PropertyInfo> GetLists(Type t)
        {
            return CachedProperties[t]
               .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
               .ToArray();
        }

        private static IReadOnlyCollection<PropertyInfo> GetProperties(Type t)
        {
            return t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(p => !(p.GetAttribute<HiddenAttribute>() is null) || p.Name != "Id")
               .ToArray();
        }

        private static Type GetType(PropertyInfo property) =>
            property.PropertyType.IsGenericType
                ? property.PropertyType.GetGenericArguments().First()
                : property.PropertyType;

        private static Attribute? GetAttributeFromMember((MemberInfo Member, Type Type) o) =>
            o.Member.GetCustomAttribute(o.Type);

        private static Attribute? GetAttributeFromEnum((Enum @enum, Type attribute) o)
        {
            var (@enum, attribute) = o;
            var enumType = @enum.GetType();
            string name = Enum.GetName(enumType, @enum);
            if (name is null)
                return null;

            var field = enumType.GetField(name);
            return field is null ? null : GetAttributeFromMember((field, attribute));
        }

        public static T? GetAttribute<T>(this MemberInfo member) where T : Attribute =>
            AttributeCache[(member, typeof(T))] as T;

        public static Type GetRealType(this PropertyInfo property) => TypeCache[property];

        public static IReadOnlyCollection<PropertyInfo> GetPrimitives<T>() => CachedPrimitives[typeof(T)];

        public static IReadOnlyCollection<PropertyInfo> GetLists<T>() => CachedLists[typeof(T)];

        public static IReadOnlyCollection<PropertyInfo> GetProperties<T>() => CachedProperties[typeof(T)];

        public static T? GetAttributeOfEnum<T>(this Enum obj) where T : Attribute =>
            EnumAttributeCache[(obj, typeof(T))] as T;
    }
}