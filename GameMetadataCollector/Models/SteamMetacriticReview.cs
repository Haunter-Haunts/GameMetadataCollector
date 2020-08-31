using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMetadataCollector.Models
{
    class SteamMetacriticReview
    {
        [JsonProperty("score")]
        public int Score { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
