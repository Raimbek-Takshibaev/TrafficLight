using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Observer.Models
{
    class ErrorResponse : Response
    {
        [JsonProperty(PropertyName = "msg")]
        public string Message { get; set; }
    }
}
