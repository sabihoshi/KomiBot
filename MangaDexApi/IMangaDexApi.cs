using System.Threading.Tasks;
using MangaDexApi.Models;
using Refit;

namespace MangaDexApi
{
    public interface IMangaDexApi
    {
        [Get("/manga/{id}")]
        Task<Title> GetTitle(int id);

        [Get("/chapter/{id}")]
        Task<Chapter> GetChapter(int id);
    }
}