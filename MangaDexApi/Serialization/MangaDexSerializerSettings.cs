using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MangaDexApi.Serialization
{
    public class MangaDexSerializerSettings : JsonSerializerSettings
    {
        public MangaDexSerializerSettings() =>
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
    }
}