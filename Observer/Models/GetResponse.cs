using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Observer.Models
{
    class GetResponse : Response
    {
        [JsonProperty(PropertyName = "response")]
        public TrafficLightModel TrafficLight { get; set; }
    }
}
