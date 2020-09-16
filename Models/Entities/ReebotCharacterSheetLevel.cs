using System.Collections.Generic;
using Newtonsoft.Json;

namespace Reebot.Models.Entities
{
    public class ReebotCharacterSheetLevel
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("levels")]
        public List<Levels> Levels { get; set; }
    }

    public struct Levels
    {
                
        [JsonProperty("level")]
        public long Level { get; set; }
        
        [JsonProperty("start_xp")]
        public long StartXp { get; set; }
        
        [JsonProperty("max_xp")]
        public long MaxXp { get; set; }
    }
}