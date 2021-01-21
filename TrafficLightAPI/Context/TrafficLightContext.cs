using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficLightAPI.Models;

namespace TrafficLightAPI.Context
{
    public class TrafficLightContext : DbContext
    {
        public DbSet<Sequence> Sequences { get; set; }
        public DbSet<Observation> Observations { get; set; }
        public TrafficLightContext(DbContextOptions<TrafficLightContext> options) :
            base(options)
        { }
    }
}
