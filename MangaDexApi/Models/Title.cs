using System.Collections.Generic;

namespace MangaDexApi.Models
{
    public class Title
    {
        public Manga Manga { get; set; }

        public Dictionary<string, TitleChapter> Chapter { get; set; }

        public Dictionary<string, Group> Group { get; set; }

        public string Status { get; set; }
    }
}
