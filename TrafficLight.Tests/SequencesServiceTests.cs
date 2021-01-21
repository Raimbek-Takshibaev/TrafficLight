using System;
using System.Collections.Generic;
using System.Text;
using TrafficLightAPI.Models;
using TrafficLightAPI.Services;
using Xunit;

namespace TrafficLight.Tests
{
    public class SequencesServiceTests
    {
        [Fact]
        public void GetClosestValuesTest()
        {
            SequencesService sequencesService = new SequencesService();

            int[] result = sequencesService.GetClosestValues(new string[2] { "1110111", "0011101" });

            Assert.Equal(new int[4] { 2, 8, 82, 88 }, result);
        }
        [Fact]
        public void GetIndexOfClockTest()
        {
            SequencesService sequncesService = new SequencesService();

            int result = sequncesService.GetIndexOfClock("1110111");

            Assert.Equal(0, result);
        }
        [Fact]
        public void ToIntTest()
        {
            SequencesService sequncesService = new SequencesService();

            int result = sequncesService.ToInt(new string[] { "1011101", "1110111" });

            Assert.Equal(20, result);
        }

        [Fact]
        public void GetTrueClockTest()
        {
            SequencesService sequncesService = new SequencesService();

            bool result = sequncesService.GetTrueClockValide(new string[] { "1110111", "0011101" }, new string[] { "0000000", "1000010" }, new string[] { "1110111", "1011101" });

            Assert.True(result);
        }

        [Fact]
        public void SetObservationClocksTest()
        {
            SequencesService sequncesService = new SequencesService();
            Observation observation = new Observation();
            Sequence sequence = new Sequence();

            sequncesService.SetObservationClocks(
                ref observation, 
                ref sequence,
                new string[2] { "1110111", "1011101" },
                new string[2] { "1110111", "0011101" }
                );


            Assert.Equal("1110111 1011101", observation.NumbersStr);
            Assert.Equal("0000000 1000000", sequence.NickedBrokenNumbersStr);
        }

        [Fact]
        public void GetCurrentClockFromPreTest()
        {
            SequencesService sequncesService = new SequencesService();

            string[] result = sequncesService.GetCurrentClockFromPre(new string[] { "1011101", "1110111" });

            Assert.NotNull(result);
            Assert.Equal("0010010", result[0]);
            Assert.Equal("1111011", result[1]);
        }

        [Fact]
        public void ConvertClocksTest()
        {
            SequencesService sequncesService = new SequencesService();

            string[] result = sequncesService.ConvertClocks(90);

            Assert.NotNull(result);
            Assert.Equal("1111011", result[0]);
            Assert.Equal("1110111", result[1]);
        }

        [Fact]
        public void GetMissingSectionsTest()
        {
            SequencesService sequncesService = new SequencesService();

            string firstClock = sequncesService.GetMissingSections
                (
                    "0011101",
                    2,
                    "1000010",
                    "0000000"
                );
            string secondClock = sequncesService.GetMissingSections
                (
                    "0010000",
                    1,
                    "1000010",
                    firstClock
                );

            Assert.False(String.IsNullOrEmpty(firstClock));
            Assert.False(String.IsNullOrEmpty(secondClock));
            Assert.Equal("1000000", firstClock);
            Assert.Equal("1000010", secondClock);
        }
    }
}
