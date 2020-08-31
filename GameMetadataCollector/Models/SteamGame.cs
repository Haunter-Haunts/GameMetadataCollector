using Newtonsoft.Json;

namespace GameMetadataCollector.Models
{
    class SteamGame
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("controller_support")]
        public string ControllerSupport { get; set; }
        [JsonProperty("detailed_description")]
        public string DetailedDescription { get; set; }
        [JsonProperty("header_image")]
        public string HeaderImage { get; set; }
        [JsonProperty("developers")]
        public string[] Developers { get; set; }
        [JsonProperty("publishers")]
        public string[] Publishers { get; set; }
        [JsonProperty("metacritic")]
        public SteamMetacriticReview Metacritic { get; set; }
        [JsonProperty("screenshots")]
        public SteamScreenshot[] Screenshots { get; set; }
        [JsonProperty("genres")]
        public SteamGenre[] Genres { get; set; }
    }
}
