using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficLightAPI.Models
{
    public class ObservationRequest
    {
        [JsonProperty(PropertyName = "sequence")]
        public string Sequence { get; set; }
        [JsonProperty(PropertyName = "observation")]
        public UserObservation Observation{ get; set; }
    }
}
