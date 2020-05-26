using System;
using MangaDexApi.Serialization;
using Refit;

namespace MangaDexApi
{
    public static class MangaDexApiFactory
    {
        public static IMangaDexApi Create(IServiceProvider arg) =>
            RestService.For<IMangaDexApi>("https://mangadex.org/api",
                new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer(
                        new MangaDexSerializerSettings())
                });
    }
}