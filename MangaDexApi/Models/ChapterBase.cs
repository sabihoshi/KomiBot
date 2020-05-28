namespace MangaDexApi.Models
{
    public class ChapterBase : IResponse
    {
        public int? Volume { get; set; }

        public string? Chapter { get; set; }

        public string? Title { get; set; }

        public string? LangCode { get; set; }

        public int? GroupId { get; set; }

        public int? GroupId2 { get; set; }

        public int? GroupId3 { get; set; }

        public int? Timestamp { get; set; }

        public string Status { get; set; }

        public string? Comment { get; set; }
    }
}