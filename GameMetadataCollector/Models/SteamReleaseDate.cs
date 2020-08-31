using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMetadataCollector.Models
{
    class SteamReleaseDate
    {
        [JsonProperty("coming_soon")]
        public bool ComingSoon { get; set; }
        [JsonProperty("date")]
        public bool Date { get; set; }
    }
}
