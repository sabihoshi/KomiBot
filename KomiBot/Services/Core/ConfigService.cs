using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace KomiBot.Services.Core
{
    public class ConfigService
    {
        private static string GetPath(MemberInfo type)

        {
            return type.Name.ToLowerInvariant() + ".json";
        }

        public static T GetJson<T>()
        {
            var path = GetPath(typeof(T));
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}