using Microsoft.EntityFrameworkCore;
using System;
using TrafficLightAPI.Context;
using TrafficLightAPI.Models;

namespace TrafficLight.Tests
{
    public class DbTest
    {
        public Sequence sequence;
        protected DbContextOptions<TrafficLightContext> ContextOptions { get; }

        protected DbTest(DbContextOptions<TrafficLightContext> contextOptions)
        {
            ContextOptions = contextOptions;

            Seed();
        }

        private void Seed()
        {
            using (var context = new TrafficLightContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Sequence sequence = new Sequence();
                this.sequence = sequence;
                context.Add(sequence);
                context.SaveChanges();
            }
        }
    }
}
