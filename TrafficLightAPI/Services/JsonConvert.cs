using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficLightAPI.Services
{
    public class JsonConvert
    {
        public static string Json(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        public static string BadRequestJson(string msg)
        {
            return $"{{'status': 'error', response: '{msg}'}}";
        }
    }
}
