using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficLightAPI.Context;
using TrafficLightAPI.Controllers;
using TrafficLightAPI.Models;
using TrafficLightAPI.Services;
using Xunit;

namespace TrafficLight.Tests
{
    public class ObservationsControllerTests : DbTest
    {
        public ObservationsControllerTests()
        : base(
            new DbContextOptionsBuilder<TrafficLightContext>()
                .UseSqlite("Filename=Test.db")
                .Options)
        {
        }
        [Fact]
        public async Task CrushTest()
        {
            using(var context = new TrafficLightContext(ContextOptions))
            {
                SequencesController seqController = new SequencesController(context, new SequencesService());
                ObservationsController obsController = new ObservationsController(context, new SequencesService(), new ObservationsService(context));


                string sequenceJson = await seqController.Create();
                var addSequenceResponseType = new { status = "", response = new { sequence = "" } };
                var addSequenceResponse = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(sequenceJson, addSequenceResponseType);
                string getSequenceJson = seqController.Get(addSequenceResponse.response.sequence);
                var getSequenceResponseType = new { status = "", response = new { clock = new string[2], color = "" } };
                var getSequenceResponse = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(getSequenceJson, getSequenceResponseType);
                string obsJson = await obsController.Create(new ObservationRequest()
                {
                    Sequence = addSequenceResponse.response.sequence,
                    Observation = new UserObservation()
                    {
                        Color = getSequenceResponse.response.color,
                        Numbers = getSequenceResponse.response.clock
                    }
                });
                Assert.Equal("green", getSequenceResponse.response.color);
                var addObsResponseType = new { status = "", response = new { missing = new string[2], numbers = new int[2]} };
                var addObsResponse = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(obsJson, addObsResponseType);

            }
        }
        [Fact]
        public void CreateTest()
        {
            using (var context = new TrafficLightContext(ContextOptions))
            {
                var controller = new ObservationsController(context, new SequencesService(), new ObservationsService(context));

                ObservationRequest request = new ObservationRequest()
                {
                    Sequence = sequence.Id,
                    Observation = new UserObservation() { Color = sequence.StartColor, Numbers = new string[2] { "1110111", "0011101" } }                
                };
                string firstResponseJson = controller.Create(request).Result;
                var responseType = new { status = "", response = new { start = new int[1], missing = new string[1] } };
                var firstResponse = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(firstResponseJson, responseType);
                Thread.Sleep(1000);
                request.Observation.Numbers[1] = "0010000";
                var secondResponseJson = controller.Create(request).Result;
                var secondResponse = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(secondResponseJson, responseType);
                Thread.Sleep(1000);
                request.Observation.Numbers = null;
                request.Observation.Color = "red";
                var redResponseJson = controller.Create(request).Result;
                var redResponse = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(redResponseJson, responseType);

                Assert.NotNull(firstResponse.response);
                Assert.NotNull(firstResponse.response.missing);
                Assert.Equal(new int[4] { 2, 8, 82, 88}, firstResponse.response.start);
                Assert.Equal(2, firstResponse.response.missing.Length);
                Assert.Equal("1000000", firstResponse.response.missing[1]);
                Assert.Equal("0000000", firstResponse.response.missing[0]);
                Assert.Equal("ok", firstResponse.status);
                Assert.NotNull(secondResponse.response);
                Assert.NotNull(secondResponse.response.missing);
                Assert.Equal("1000010", secondResponse.response.missing[1]);
                Assert.Equal("ok", secondResponse.status);
                Assert.NotNull(redResponse.response);
                Assert.NotNull(redResponse.response.missing);
                Assert.Equal("1000010", redResponse.response.missing[1]);
                Assert.Equal("ok", redResponse.status);
            }
        }
    }
}
