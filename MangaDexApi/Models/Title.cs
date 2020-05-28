using System.Collections.Generic;

namespace MangaDexApi.Models
{
    public class Title : IResponse
    {
        public Manga Manga { get; set; }

        public Dictionary<string, TitleChapter> Chapter { get; set; }

        public Dictionary<string, Group> Group { get; set; }

        public string Status { get; set; }

        public string? Comment { get; set; }

        public class TitleChapter : ChapterBase
        {
            public string GroupName { get; set; }

            public string GroupName2 { get; set; }

            public string GroupName3 { get; set; }
        }
    }
}
