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

        private static string GetTableName<T>()
        {
            var tableName = typeof(T).Name;
            if (typeof(T).IsInterface && tableName.StartsWith('I'))
                tableName = tableName.Substring(1);
            return tableName;
        }

        public bool TrySetValue<TClass>(TClass obj, string property, string value)
        {
            if (!CanAssign<TClass>(property, value))
                return false;

            var p = typeof(TClass).GetProperty(property,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (p is null)
                return false;

            return Type.GetTypeCode(p.PropertyType) switch
            {
                TypeCode.String => SetValue(value),
                TypeCode.Int32 => SetValue(int.Parse(value)),
                TypeCode.Int64 => SetValue(long.Parse(value)),
                TypeCode.UInt32 => SetValue(uint.Parse(value)),
                TypeCode.UInt64 => SetValue(ulong.Parse(value)),
                TypeCode.Boolean => SetValue(bool.Parse(value)),
                _ => false
            };

            bool SetValue(object v)
            {
                p.SetValue(obj, v);
                return true;
            }
        }

        public static bool CanAssign<TClass>(string property, string value)
        {
            var type = typeof(TClass).GetProperty(property,
                                          BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                                    ?.PropertyType;

            if (type is null)
                return false;

            if (type == typeof(string))
                return true;

            return ConvertDictionary.ContainsKey(type) && ConvertDictionary[type](value);
        }

        public static Dictionary<Type, Func<string, bool>> ConvertDictionary { get; } =
            new Dictionary<Type, Func<string, bool>>
            {
                { typeof(int), s => int.TryParse(s, out _) },
                { typeof(ulong), s => ulong.TryParse(s, out _) }
            };

        public bool TryGetGuildData<T>(IGuild guild, out T data, string tableName = null) where T : class, IGuildData
        {
            var collection = GetTableData<T>(tableName);
            data = collection.FindOne(c => c.Id == guild.Id);
            return data != null;
        }

        public T EnsureGuildData<T>(IGuild guild, string tableName = null) where T : class, IGuildData, new()
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