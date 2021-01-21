using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficLightAPI.Context;
using TrafficLightAPI.Models;

namespace TrafficLightAPI.Services
{
    public class ObservationsService
    {
        private TrafficLightContext _db;

        public ObservationsService(TrafficLightContext db)
        {
            _db = db;
        }
        public string CheckObservationValid(ObservationRequest observationRequest)
        {
            if (String.IsNullOrEmpty(observationRequest.Observation.Color))
                return "Invalid color";
            if (observationRequest.Observation.Numbers is null && observationRequest.Observation.Color != "red")
                return "Invalid numbers";
            if (observationRequest.Observation.Color != "green" && observationRequest.Observation.Color != "red")
                return "Invalid color";
            if (observationRequest.Observation.Color == "red" && observationRequest.Observation.Numbers != null)
                return "There isn't enough data";
            else if(observationRequest.Observation.Color == "red")
                return "ok";
            if (observationRequest.Observation.Color == "green" && !CheckValidClock(observationRequest.Observation.Numbers))
                return "Invalid numbers";
            else if (String.IsNullOrEmpty(observationRequest.Sequence))
                return "The sequence is null";
            else if (_db.Observations.Any(o => o.Id == $"{observationRequest.Sequence}{observationRequest.Observation.Numbers[0]}{observationRequest.Observation.Numbers[1]}"))
                return "No solutions found";           
            return "ok";
        }
        public bool CheckValidClock(string[] clocks)
        {
            if (clocks is null || clocks.Length != 2)
                return false;
            foreach (var clock in clocks)
            {
                if (clock.Length != 7)
                    return false;
                foreach (char section in clock)
                {
                    if (section != '1' && section != '0')
                        return false;
                }
            }
            return true;
        }
    }
}
