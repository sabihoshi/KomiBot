using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace KomiBot.Services.Core
{
    public class ConfigService
    {
        private static string GetPath(MemberInfo type) => type.Name.ToLowerInvariant() + ".json";

        public static T GetJson<T>()
        {
            string path = GetPath(typeof(T));
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}