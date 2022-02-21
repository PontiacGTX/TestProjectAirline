using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Flights> Flights { get; set; }
        public DbSet<Journeys> Journeys { get; set; }
        public DbSet<Transports> Transports { get; set; }
        public DbSet<JourneyFlights> JourneyFlights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<JourneyFlights>().HasKey(k => new { k.IdFlight, k.IdJourney});
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
