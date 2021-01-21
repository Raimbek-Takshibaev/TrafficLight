using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Observer.Models
{
    class ObsResponse : Response
    {
        [JsonProperty(PropertyName = "response")]
        public MissingAndStartResponse Response { get; set; }
    }
}
