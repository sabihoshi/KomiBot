using System;
using Newtonsoft.Json;

namespace MangaDexApi.Models
{
    public class Chapter
    {
        public int Id { get; set; }

        public int Timestamp { get; set; }

        public string Hash { get; set; }

        public int? Volume { get; set; }

        [JsonProperty("chapter")]
        public string ChapterNumber { get; set; }

        public string Title { get; set; }

        public string LangName { get; set; }

        public string LangCode { get; set; }

        public int MangaId { get; set; }

        public int GroupId { get; set; }

        public int? GroupId2 { get; set; }

        public int? GroupId3 { get; set; }

        public int Comments { get; set; }

        public Uri Server { get; set; }

        public string[] PageArray { get; set; }

        public int LongStrip { get; set; }

        public string Status { get; set; }
    }
}