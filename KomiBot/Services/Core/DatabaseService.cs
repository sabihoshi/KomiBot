using System;
using System.Collections.Generic;
using System.Reflection;
using Discord;
using KomiBot.Services.Guild;
using LiteDB;

namespace KomiBot.Services.Core
{

    public class DatabaseService
    {
        private readonly ApplicationService _applicationService;

        public DatabaseService(ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public LiteCollection<T> GetTableData<T>(string? tableName = null)
        {
            tableName ??= GetTableName<T>();

            using var db = new LiteDatabase(_applicationService.ConnectionString);
            return db.GetCollection<T>(tableName);
        }

        private static string GetTableName<T>(Type? type = null)
        {
            type ??= typeof(T);
            var tableName = type.Name;
            if (type.IsInterface && tableName.StartsWith('I'))
                tableName = tableName.Substring(1);
            return tableName;
        }

        public static bool TryGetProperty(Type type, string propertyName, string stringValue, out PropertyInfo? property, out object? value)
        {
            value = null;

            property = type.GetProperty(propertyName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property is null)
                return false;

            var nullable = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

            var propertyType = nullable ? Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType;

            if (propertyType is null)
                return false;

            if (!ConvertDictionary.TryGetValue(propertyType, out var tryParse))
                return false;

            return tryParse(stringValue, out value);
        }

        public delegate bool TryParse<T>(string value, out T result);

        public delegate bool TryParseObject(string value, out object result);

        public static Dictionary<Type, TryParseObject> ConvertDictionary { get; } =
            new Dictionary<Type, TryParseObject>
            {
                [typeof(string)] = (string s, out object v) => FromQualifiedTryParse(true, s, out v),
                [typeof(int)] = (string s, out object v) => FromQualifiedTryParse(int.TryParse(s, out var result), result, out v)
                
            };

        private static bool FromQualifiedTryParse<T>(bool success, T value, out object newValue)
        {
            newValue = value;
            return success;
        }

        public bool TryGetGuildData<T>(IGuild guild, out T data, string tableName = null) where T : class, IGuildData
        {
            var collection = GetTableData<T>(tableName);
            data = collection.FindOne(c => c.Id == guild.Id);
            return data != null;
        }

        public T EnsureGuildData<T>(IGuild guild, string? tableName = null) where T : class, IGuildData, new()
        {
            if (!TryGetGuildData<T>(guild, out var data, tableName))
            {
                data = new T { Id = guild.Id };
                GetTableData<T>().Insert(data);
            }

            return data;
        }
    }
}