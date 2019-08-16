using Discord;
using KomiBot.Models;
using KomiBot.Models.GuildData;
using LiteDB;

namespace KomiBot.Services
{
    public class DatabaseService
    {
        public Application? Application { get; set; }

        private LiteCollection<T> GetTableData<T>(string? tableName = null)
        {
            tableName ??= GetTableName<T>();

            using var db = new LiteDatabase(Application?.ConnectionString);
            return db.GetCollection<T>(tableName);
        }

        private static string GetTableName<T>()
        {
            var tableName = typeof(T).Name;
            if (typeof(T).IsInterface && tableName.StartsWith('I'))
                tableName = tableName.Substring(1);
            return tableName;
        }

        public bool TryGetGuildData<T>(IGuild guild, out T data) where T : class, IGuildData, new()
        {
            var collection = GetTableData<T>();
            data = collection.FindOne(c => c.GuildId == guild.Id);
            return data != null;
        }

        public T EnsureGuildData<T>(IGuild guild) where T : class, IGuildData, new()
        {
            if (!TryGetGuildData<T>(guild, out var data))
            {
                data = new T { GuildId = guild.Id };
                GetTableData<T>().Insert(data);
            }

            return data;
        }
    }
}