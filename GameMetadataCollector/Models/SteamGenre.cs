using Newtonsoft.Json;

namespace GameMetadataCollector.Models
{
    class SteamGenre
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
