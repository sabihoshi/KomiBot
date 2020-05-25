using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace MangaDexApi
{
    public static class MangaDexApiFactory
    {
        public static IMangaDexApi Create() =>
            RestService.For<IMangaDexApi>("https://mangadex.org/api",
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer(
                        new JsonSerializerSettings
                        {
                            ContractResolver = new DefaultContractResolver
                            {
                                NamingStrategy = new SnakeCaseNamingStrategy()
                            }
                        }
                    )
                });
    }
}