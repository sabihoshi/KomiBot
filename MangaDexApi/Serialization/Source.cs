using Newtonsoft.Json;

namespace MangaDexApi.Serialization
{
    public class Source
    {
        [JsonConstructor]
        public Source(SourceType type, string name, string? baseUrl = null, string? emoji = null)
        {
            Type = type;
            Name = name;
            BaseUrl = baseUrl;
            Emoji = emoji ?? name;
        }

        public enum SourceType
        {
            Retail,
            Information,
            Official
        }

        public SourceType Type { get; set; }

        public string Name { get; set; }

        public string? BaseUrl { get; set; }

        public string Link { get; set; }

        public string Emoji { get; set; }

        public string Get() => BaseUrl + Link;
    }
}