namespace MangaDexApi.Models
{
    public class TitleChapter
    {
        public int? Volume { get; set; }

        public string Chapter { get; set; }

        public string Title { get; set; }

        public string LangCode { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int GroupId2 { get; set; }

        public string GroupName2 { get; set; }

        public int GroupId3 { get; set; }

        public string GroupName3 { get; set; }

        public int Timestamp { get; set; }
    }
}