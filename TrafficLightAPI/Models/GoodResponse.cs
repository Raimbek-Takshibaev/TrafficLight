using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficLightAPI.Models
{
    public class GoodResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; } = "ok";
        [JsonProperty(PropertyName = "response")]
        public object Response { get; set; }
    }
}
