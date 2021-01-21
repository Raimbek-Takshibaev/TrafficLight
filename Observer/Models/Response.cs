using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Observer.Models
{
    class Response
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }
}
