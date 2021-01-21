using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Observer.Models
{
    class MissingAndStartResponse
    {
        [JsonProperty(PropertyName = "start")]
        public int[] Start { get; set; }
        [JsonProperty(PropertyName = "missing")]
        public string[] Missing { get; set; }
    }
}
