using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Observer.Models
{
    class TrafficLightModel
    {
        [JsonProperty(PropertyName = "clock")]
        public string[] Clock { get; set; }
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }
    }
}
