using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Observer.Models
{
    class AddResponse : Response
    {
        [JsonProperty(PropertyName = "response")]
        public SequenceModel Response { get; set; }
    }
}
