using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Observer.Models
{
    class Request
    {
        [JsonProperty(PropertyName = "sequence")]
        public string Sequence { get; set; }
        [JsonProperty(PropertyName = "observation")]
        public Observation Observation { get; set; }
    }
}
