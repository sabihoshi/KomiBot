using Newtonsoft.Json;

namespace MangaDexApi.Models
{
    public class Group
    {
        [JsonProperty("group_name")]
        public string Name { get; set; }
    }
}