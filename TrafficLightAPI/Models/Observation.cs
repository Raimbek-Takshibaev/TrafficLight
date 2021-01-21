using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficLightAPI.Models
{
    public class Observation
    {
        public string Id { get; set; }
        public string SequenceId { get; set; }
        public string NumbersStr { get; set; } = "0000000 0000000";
        public string Color { get; set; } = "green";
        public Sequence Sequence { get; set; }
        [NotMapped]
        public string[] Numbers 
        { 
            get
            {
                return NumbersStr.Split(new char[] { ' ' });
            }
            set
            {
                NumbersStr = $"{value[0]} {value[1]}";
            }
        }
    }
}
