using System.Collections.Generic;
using MangaDexApi.Serialization;
using Newtonsoft.Json;

namespace MangaDexApi.Models
{
    public class Manga
    {
        public string CoverUrl { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Author { get; set; }

        public int Status { get; set; }

        public int[] Genres { get; set; }

        public int LastChapter { get; set; }

        public string LangName { get; set; }

        public string LangFlag { get; set; }

        public int Hentai { get; set; }

        [JsonConverter(typeof(SourceConverter))]
        public IEnumerable<Source> Links { get; set; }
    }
}