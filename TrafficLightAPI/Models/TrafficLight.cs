using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficLightAPI.Models
{
    public class TrafficLight
    {
        [JsonProperty(PropertyName = "clock")]
        public string[] Clock { get; set; }
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }
    }
}
