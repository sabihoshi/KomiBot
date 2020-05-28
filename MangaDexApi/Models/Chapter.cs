using System;
using Newtonsoft.Json;

namespace MangaDexApi.Models
{
    public class Chapter : ChapterBase
    {
        public int Id { get; set; }

        public string? Hash { get; set; }

        [JsonProperty("chapter")]
        public string? ChapterNumber { get; set; }

        public string? LangName { get; set; }

        public int? MangaId { get; set; }

        public int? Comments { get; set; }

        public Uri? Server { get; set; }

        public string[]? PageArray { get; set; }

        public int? LongStrip { get; set; }
    }
}