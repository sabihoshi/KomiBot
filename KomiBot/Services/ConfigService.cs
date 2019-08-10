﻿using System.IO;
using Newtonsoft.Json;
using System.Reflection;

namespace KomiBot.Services
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