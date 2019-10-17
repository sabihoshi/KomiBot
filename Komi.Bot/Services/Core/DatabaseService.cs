using System;
using System.Diagnostics.CodeAnalysis;
using Discord;
using Komi.Data.Models.Settings;
using LiteDB;

namespace Komi.Bot.Services.Core
{
    public interface IDatabaseService
    {
        LiteCollection<T> GetTableData<T>(string? tableName = null);

        bool TryGetGuildData<T>(
            IGuild guild,
            [MaybeNullWhen(false)] out T data,
            string? tableName = null) where T : class, IGuildData;

        T EnsureGuildData<T>(IGuild guild, string? tableName = null) where T : class, IGuildData, new();

        void UpdateData<T>(T update);
    }

    public class DatabaseService
        : IDatabaseService
    {
        private readonly ApplicationService _applicationService;

        public DatabaseService(ApplicationService applicationService) => _applicationService = applicationService;

        public LiteCollection<T> GetTableData<T>(string? tableName = null)
        {
            tableName ??= GetTableName<T>();

            using var db = new LiteDatabase(_applicationService.ConnectionString);
            return db.GetCollection<T>(tableName);
        }

        public static string GetTableName<T>(Type? type = null)
        {
            type ??= typeof(T);
            string tableName = type.Name;
            if (type.IsInterface && tableName.StartsWith('I'))
                tableName = tableName.Substring(1);
            return tableName;
        }

        public bool TryGetGuildData<T>(
            IGuild guild,
            out T data,
            string? tableName = null) where T : class, IGuildData
        {
            var collection = GetTableData<T>(tableName);
            data = collection.FindOne(c => c.Id == guild.Id);
            return data != null;
        }

        public T EnsureGuildData<T>(IGuild guild, string? tableName = null) where T : class, IGuildData, new()
        {
            if (!TryGetGuildData(guild, out T data, tableName))
            {
                data = new T { Id = guild.Id };
                GetTableData<T>().Insert(data);
            }

            return data;
        }

        public void UpdateData<T>(T update) => GetTableData<T>().Upsert(update);
    }
}