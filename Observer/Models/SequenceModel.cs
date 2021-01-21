using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Observer.Models
{
    class SequenceModel
    {
        [JsonProperty(PropertyName = "sequence")]
        public string Sequence { get; set; }
    }
}
