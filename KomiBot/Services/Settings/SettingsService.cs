using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KomiBot.Core.Attributes;
using KomiBot.Services.Utilities;

namespace KomiBot.Services.Settings
{
    public class SettingsService
    {
        private static readonly Cache<Type, IReadOnlyCollection<PropertyInfo>> CachedProperties =
            new Cache<Type, IReadOnlyCollection<PropertyInfo>>(GetProperties);

        private static readonly Cache<Type, IReadOnlyDictionary<string, PropertyInfo>> PropertyCache =
            new Cache<Type, IReadOnlyDictionary<string, PropertyInfo>>(GetPropDict);

        public static Cache<PropertyInfo, Type?> TypeCache { get; } = new Cache<PropertyInfo, Type?>(GetRealType);

        private static IReadOnlyCollection<PropertyInfo> GetProperties(Type t)
        {
            return t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<HiddenAttribute>() is null && p.Name != "Id")
                    .Where(p => !(p.PropertyType.IsGenericType &&
                                  p.PropertyType.GetGenericTypeDefinition() == typeof(List<>)))
                    .ToArray();
        }

        private static IReadOnlyDictionary<string, PropertyInfo> GetPropDict(Type t)
        {
            return CachedProperties[t].ToDictionary(p => p.Name.ToLower(), p => p);
        }

        private static Type? GetRealType(PropertyInfo property)
        {
            var isGeneric = property.PropertyType.IsGenericType &&
                            property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            return isGeneric
                ? Nullable.GetUnderlyingType(property.PropertyType)
                : property.PropertyType;
        }

        public bool TryGetProperty<T>(string propertyName, string stringValue, out PropertyInfo? property,
            out object? value)
        {
            value = null;

            if (!PropertyCache[typeof(T)].TryGetValue(propertyName.ToLower(), out property))
                return false;

            var propertyType = TypeCache[property];

            if (propertyType is null)
                return false;

            if (!ConvertDictionary.TryGetValue(propertyType, out var tryParse))
                return false;

            return tryParse(stringValue, out value);
        }

        public delegate bool TryParseObject(string value, out object result);

        public Dictionary<Type, TryParseObject> ConvertDictionary { get; } =
            new Dictionary<Type, TryParseObject>
            {
                [typeof(string)] = (string s, out object v) => FromQualifiedTryParse(true, s, out v),
                [typeof(int)] = (string s, out object v) =>
                    FromQualifiedTryParse(int.TryParse(s, out var result), result, out v)
            };

        private static bool FromQualifiedTryParse<T>(bool success, T value, out object newValue)
        {
            newValue = value;
            return success;
        }

        public IReadOnlyCollection<PropertyInfo> GetProperties<T>()
        {
            return CachedProperties[typeof(T)];
        }
    }
}