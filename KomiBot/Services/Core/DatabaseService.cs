﻿using System;
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

        public static bool TrySetValue<TClass>(TClass obj, string property, string value)
        {
            var (canAssign, p) = CanAssign<TClass>(property, value);

            if (!canAssign || p is null)
                return false;

            var (_, newValue) = ConvertDictionary[p.PropertyType](value);

            p.SetValue(obj, newValue);

            return true;
        }

        public static (bool, PropertyInfo?) CanAssign<TClass>(string property, string value)
        {
            var p = typeof(TClass).GetProperty(property,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (p is null)
                return (false, null);

            var type = p.PropertyType;

            if (type == typeof(string))
                return (true, p);

            bool canAssign = ConvertDictionary.ContainsKey(type) && ConvertDictionary[type](value).Item1;

            return (canAssign, p);
        }

        public static Dictionary<Type, Func<string, (bool, object)>> ConvertDictionary { get; } =
            new Dictionary<Type, Func<string, (bool, object)>>
            {
                {
                    typeof(int), s =>
                    {
                        var success = int.TryParse(s, out var intResult);
                        return (success, intResult);
                    }
                },
                {
                    typeof(ulong), s =>
                    {
                        var success = ulong.TryParse(s, out var intResult);
                        return (success, intResult);
                    }
                }
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