using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMetadataCollector.Models
{
    class SteamScreenshot
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("path_thumbnail")]
        public string PathThumbnail { get; set; }
        [JsonProperty("path_full")]
        public string PathFull { get; set; }
    }
}
