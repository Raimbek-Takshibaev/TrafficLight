using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrafficLightAPI.Context;
using TrafficLightAPI.Models;
using TrafficLightAPI.Services;

namespace TrafficLightAPI.Controllers
{
    [Route("sequence")]
    [ApiController]
    public class SequencesController : ControllerBase
    {
        private TrafficLightContext _db;
        private SequencesService _sequencesService;

        public SequencesController(TrafficLightContext db, SequencesService sequencesService)
        {
            _db = db;
            _sequencesService = sequencesService;
        }
        [HttpGet("get")]
        public string Get(string id)
        {
            Sequence sequence = _db.Sequences.Find(id);
            Observation preObservation = _db.Observations.AsEnumerable().LastOrDefault(o => o.SequenceId == id);
            if (sequence is null)
                return "not found";
            TrafficLight trafficLight = new TrafficLight();
            if (preObservation is null)
            {
                if (sequence.StartColor == "red")
                {
                    DateTime now = DateTime.Now;
                    now.AddMilliseconds(-now.Millisecond);
                    int seconds = (int)(now - sequence.CatheringDate).TotalSeconds;
                    if (seconds > sequence.StartClock)
                    {
                        trafficLight.Color = "green";
                        trafficLight.Clock = _sequencesService.GetBrokenClock(_sequencesService.ConvertClocks(sequence.MaxClock), sequence.BrokenNumbers);
                    }
                    else
                        trafficLight.Color = "red";
                }
                else
                {
                    trafficLight.Color = "green";
                    trafficLight.Clock = _sequencesService.GetBrokenClock(_sequencesService.ConvertClocks(sequence.StartClock), sequence.BrokenNumbers);
                }
            }
            else
                trafficLight = _sequencesService.ConvertToTrafficLight(sequence, preObservation);
            GoodResponse goodResponse = new GoodResponse() { Response = trafficLight };
            return JsonConvert.Json(goodResponse);
        }
        [HttpPost("create")]
        public async Task<string> Create()
        {
            try
            {
                Sequence sequence = _sequencesService.GenerateSequence();
                sequence.CatheringDate = DateTime.Now;
                sequence.CatheringDate.AddMilliseconds(-sequence.CatheringDate.Millisecond);
                _db.Entry(sequence).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                await _db.SaveChangesAsync();
                GoodResponse response = new GoodResponse()
                {
                    Response = new { sequence = sequence.Id }
                };
                return JsonConvert.Json(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}