using DataAccess.HelperClass.FactoryClass;
using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class FlightsRepository
    {
        AccessSettings _AccessSettings { get; }
        protected ILogger _Logger { get; }
        protected AppDbContext _AppDbContext { get; }
        public FlightsRepository(AppDbContext ctx, ILogger logger,AccessSettings accessSettings)
        {
            _AccessSettings = accessSettings;
            _Logger = logger;
            _AppDbContext = ctx;
        }

        public async Task<Flights> GetFlight(Func<Flights, bool> selector) => 
           await _AppDbContext.Flights.Where(x => selector(x)).Include(x=>x.Transport).FirstOrDefaultAsync();

        public async Task<IList<Journey>> GetJourney(OriginDestination flightOriginDestination)
        => await (from journey in _AppDbContext.Journeys
            join journeyFlights in _AppDbContext.JourneyFlights on journey.IdJourney equals journeyFlights.IdJourney
            join flights in _AppDbContext.Flights on journeyFlights.IdFlight equals flights.IdFlight
            where flights.Destination == flightOriginDestination.Origin && flights.Destination == flightOriginDestination.Destination
            select new Journey
            {
                Destination = journey.Destination,
                Origin = journey.Origin,
                Price = journey.Price
            }).Include(x => x.Flights).ThenInclude(x => x.Transport).ToListAsync();

        public async Task<Flights> InsertFlight(Flights flight)
        {
            try
            {
              
                EntityEntry<Flights> flightIn = await _AppDbContext.Flights.AddAsync(flight);
                bool saved = (await _AppDbContext.SaveChangesAsync()>0);
                return saved ? flightIn.Entity : null;
            }
            catch (Exception ex )
            {
                _Logger.LogError(Factory.GetError(string.Concat(nameof(FlightsRepository), " Repository at ", nameof(InsertFlight)),
                   string.Concat("Error:", ex.Message, " Error Details: ", ex.InnerException, "Stack Trace", ex.StackTrace, $" Parameters: {typeof(Flights)}")));
                throw;
            }
        }
        public async Task<IList<Flight>> GetFlightsByJourneyId(int idJourney)
           => await (from flight in _AppDbContext.Flights
                     join journeyFlights in _AppDbContext.JourneyFlights on flight.IdFlight equals journeyFlights.IdFlight
                     where journeyFlights.IdJourney == idJourney
                     select new Flight
                     {
                         Destination = flight.Destination,
                         Origin = flight.Origin,
                         Price = flight.Price
                     }).Include(x => x.Transport).ToListAsync();
        public async Task<IList<Flight>> GetFlightsByJourneyOriginDestination(OriginDestination originDestination)
        {
            try
            {
                return await (from flight in _AppDbContext.Flights
                              join journeyFlights in _AppDbContext.JourneyFlights on flight.IdFlight equals journeyFlights.IdFlight
                              join journey in _AppDbContext.Journeys on journeyFlights.IdJourney equals journey.IdJourney
                              where journey.Origin == originDestination.Origin && journey.Destination == originDestination.Destination
                              select new Flight
                              {
                                  Destination = flight.Destination,
                                  Origin = flight.Origin,
                                  Price = flight.Price
                              }).Include(x => x.Transport).ToListAsync();
            }
            catch (Exception ex)
            {
                _Logger.LogError(Factory.GetError(string.Concat(nameof(FlightsRepository), " Repository at ", nameof(GetFlightsByJourneyOriginDestination)),
                string.Concat("Error:", ex.Message, " Error Details: ", ex.InnerException, "Stack Trace", ex.StackTrace, $" Parameters: Origin: {originDestination.Origin} Destination: {originDestination.Destination}")));

                throw;
            }
        }




    }
}
