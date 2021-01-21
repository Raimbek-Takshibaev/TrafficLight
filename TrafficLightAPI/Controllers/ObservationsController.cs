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
    [Route("observation")]
    [ApiController]
    public class ObservationsController : ControllerBase
    {
        private TrafficLightContext _db;
        private SequencesService _sequencesService;
        private ObservationsService _observationsService;

        public ObservationsController(TrafficLightContext db, SequencesService sequencesService, ObservationsService observationsService)
        {
            _db = db;
            _sequencesService = sequencesService;
            _observationsService = observationsService;
        }

        [HttpPost("add")]
        public async Task<string> Create(ObservationRequest observation)
        {
            string msg = _observationsService.CheckObservationValid(observation);
            if (msg != "ok")
                return JsonConvert.BadRequestJson(msg);
            Sequence sequence = _db.Sequences.Find(observation.Sequence);
            if (sequence is null)
                return JsonConvert.BadRequestJson("The sequence isn't found");
            if (sequence.IsOver)
                return JsonConvert.BadRequestJson("The red observation should be the last");
            Observation newObservation = new Observation();
            Observation preObservation = _db.Observations.AsEnumerable().Reverse().FirstOrDefault(o => o.SequenceId == observation.Sequence);           
            if (observation.Observation.Color == "red")
            {
                if (preObservation is null)
                    return JsonConvert.BadRequestJson("There isn't enough data");
                if (_sequencesService.ToInt(preObservation.Numbers) == 1)                             
                    newObservation.Color = observation.Observation.Color;                                   
                else
                    return JsonConvert.BadRequestJson("No solutions found");
                GoodResponse redResponse = new GoodResponse()
                {
                    Response = new { start = new int[1] { sequence.StartClock }, missing = sequence.NickedBrokenNumbers }
                };
                newObservation.SequenceId = observation.Sequence;
                newObservation.Id = $"{observation.Sequence}red";
                _db.Entry(sequence).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.Entry(newObservation).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                await _db.SaveChangesAsync();
                return JsonConvert.Json(redResponse);
            }
            try
            {               
                newObservation.SequenceId = observation.Sequence;
                _sequencesService.SetObservationClocks(ref newObservation, ref sequence, _sequencesService.GetCurrentClockFromPre(preObservation.Numbers), observation.Observation.Numbers);
                sequence.StartClocks = _sequencesService.GetClosestValuesFromArray(sequence.StartClocks, observation.Observation.Numbers);
            }
            catch (NullReferenceException)
            {               
                newObservation = new Observation();
                string[] curClock = new string[0];
                if (sequence.StartColor == "red")
                    curClock = _sequencesService.ConvertClocks(sequence.MaxClock);
                else
                    curClock = _sequencesService.ConvertClocks(sequence.StartClock);
                if (!_sequencesService.GetTrueClockValide(observation.Observation.Numbers, sequence.BrokenNumbers, curClock))
                    return "bad request";
                _sequencesService.SetObservationClocks(ref newObservation, ref sequence, curClock, observation.Observation.Numbers);
                sequence.StartClocks = _sequencesService.GetClosestValues(observation.Observation.Numbers);
            }
            GoodResponse response = new GoodResponse()
            {
                Response = new { start = sequence.StartClocks, missing = sequence.NickedBrokenNumbers }
            };
            newObservation.SequenceId = observation.Sequence;
            newObservation.Id = $"{newObservation.SequenceId}{newObservation.Numbers[0]}{newObservation.Numbers[1]}";
            _db.Entry(sequence).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.Entry(newObservation).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            await _db.SaveChangesAsync();
            return JsonConvert.Json(response);           
        }
    }
}