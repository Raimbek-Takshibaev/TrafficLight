using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrafficLightAPI.Context;
using TrafficLightAPI.Controllers;
using TrafficLightAPI.Services;
using Xunit;

namespace TrafficLight.Tests
{
    public class SequencesControllerTests : DbTest
    {       
        public SequencesControllerTests()
        : base(
            new DbContextOptionsBuilder<TrafficLightContext>()
                .UseSqlite("Filename=Test.db")
                .Options)
        {
        }
        [Fact]
        public void CreateTest()
        {
            using (var context = new TrafficLightContext(ContextOptions))
            {
                var controller = new SequencesController(context, new SequencesService());

                string responseJson = controller.Create().Result;
                var responseType = new { status = "", response = new { sequence = ""  } };
                var response = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(responseJson, responseType);

                Assert.False(String.IsNullOrEmpty(response.response.sequence));
                Assert.Equal("ok", response.status);
            }
        }
        [Fact]
        public void GetTest()
        {
            using (var context = new TrafficLightContext(ContextOptions))
            {
                var controller = new SequencesController(context, new SequencesService());

                string responseJson = controller.Get(this.sequence.Id);
                var responseType = new { status = "", response = new { clock = new string[2], color = "" } };
                var response = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(responseJson, responseType);
                bool colorValid = response.response.color == "green" || response.response.color == "red";


                Assert.True(colorValid);
                Assert.Equal("green", response.response.color);
                Assert.Equal(2, response.response.clock.Length);
                Assert.Equal("ok", response.status);
            }
        }
    }
}
