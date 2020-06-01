using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace MangaDexApi.Serialization
{
    public class SourceConverter : JsonConverter<IEnumerable<Source>>
    {
        public static Dictionary<string, Source> SourcesJson =
            JsonConvert.DeserializeObject<Dictionary<string, Source>>(
                File.ReadAllText("./Serialization/sources.json"),
                new MangaDexSerializerSettings())!;

        public override void WriteJson(JsonWriter writer, IEnumerable<Source> value, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            foreach (var source in value)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("type");
                writer.WriteValue(source.Type);

                writer.WritePropertyName("name");
                writer.WriteValue(source.Name);

                if (source.BaseUrl != null)
                {
                    writer.WritePropertyName("base_url");
                    writer.WriteValue(source.BaseUrl);
                }

                writer.WritePropertyName("link");
                writer.WriteValue(source.Link);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        public override IEnumerable<Source> ReadJson(JsonReader reader, Type objectType,
            IEnumerable<Source> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var links = new Dictionary<string, string>();
            serializer.Populate(reader, links);
            return links
               .Select(l =>
                {
                    var source = SourcesJson[l.Key];
                    var ret = new Source(
                        source.Type, source.Name,
                        source.BaseUrl, source.Emoji)
                    {
                        Link = l.Value
                    };

                    return ret;
                });
        }
    }
}