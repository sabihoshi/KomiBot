namespace MangaDexApi.Models
{
    interface IResponse
    {
        string Status { get; set; }

        string? Comment { get; set; }
    }
}