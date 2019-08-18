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

        public bool TryGetGuildData<T>(IGuild guild, out T data, string tableName = null) where T : class, IGuildData
        {
            var collection = GetTableData<T>(tableName);
            data = collection.FindOne(c => c.GuildId == guild.Id);
            return data != null;
        }

        public T EnsureGuildData<T>(IGuild guild, string tableName = null) where T : class, IGuildData, new()
        {
            if (!TryGetGuildData<T>(guild, out var data, tableName))
            {
                data = new T { GuildId = guild.Id };
                GetTableData<T>().Insert(data);
            }

            return data;
        }
    }
}