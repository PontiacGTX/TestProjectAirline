using DataAccess.HelperClass.FactoryClass;
using DataAccess.Models;
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
    public class JourneyRepository
    {
        AccessSettings _AccessSettings { get; }
        protected ILogger _Logger { get; }
        protected AppDbContext _AppDbContext { get; }
        public JourneyRepository(AppDbContext ctx, ILogger logger, AccessSettings accessSettings)
        {
            _AccessSettings = accessSettings;
            _Logger = logger;
            _AppDbContext = ctx;
        }
        public async Task<bool> ExistJourney(OriginDestination originDestination)
        {
            try
            {
                return await _AppDbContext.Journeys.AnyAsync(x => x.Destination == originDestination.Destination && x.Origin == originDestination.Origin);
            }
            catch (Exception ex)
            {
                _Logger.LogError(Factory.GetError(string.Concat(nameof(JourneyRepository), " Repository at ", nameof(GetJourneyByFlightDestination)),
                string.Concat("Error:", ex.Message, " Error Details: ", ex.InnerException, "Stack Trace", ex.StackTrace, $" Parameters: Origin: {originDestination.Origin} Destination: {originDestination.Destination}")));

                throw;
            }
        }

        public async Task<IList<Journey>> GetJourneyByFlightDestination(OriginDestination flightOriginDestination)
        {
            try
            {
              
                var x = await _AppDbContext.Journeys.
                    Include(x => x.JourneyFlights)
                    .ThenInclude(x => x.Flights)
                    .ThenInclude(x => x.Transport)
                    .Select(x =>  new {
                        Destination = x.Destination,
                        Origin = x.Origin,
                        Flights = x.JourneyFlights.Where(x => x.Journey.Destination == flightOriginDestination.Destination && x.Journey.Origin == flightOriginDestination.Origin).Select(x => x.Flights).ToList(),
                        // Flight = x.JourneyFlights.Where(x=>x.Journey.Destination == flightOriginDestination.Destination && x.Journey.Origin == flightOriginDestination.Origin).Select(x => x.Flights).GroupBy(x => x.Index),
                        // Count = x.JourneyFlights.Where(y => y.Index == x.Index).Select(x => x.Flights).GroupBy(x => x.Index).Count()
                    }).ToListAsync();
                List<Journey> JOURNEYS = null;
                
                try
                {
                    
                    var flightList = x.Select(x => x.Flights).FirstOrDefault().GroupBy(x=>x.Index);
                    int reserve = flightList.Count();
                    JOURNEYS  = new List<Journey>();
                    int i = 0;
                    foreach (var flightsGroup in flightList)
                    {
                        Journey journey = new Journey();
                        journey.Destination = flightOriginDestination.Destination;
                        journey.Origin = flightOriginDestination.Origin;
                        journey.Flights = flightsGroup.Select(x =>  new Flight(x)).ToList();
                        journey.Price = journey.Flights.Sum(x => x.Price);
                        JOURNEYS.Add(journey);
                        i++;
                    }
                
                }
                catch (Exception ex)
                {

                }
                            
                            
                            
             
                
              return JOURNEYS;

                
            }
            catch (Exception ex)
            {
                _Logger.LogError(Factory.GetError(string.Concat(nameof(JourneyRepository), " Repository at ", nameof(GetJourneyByFlightDestination)),
                 string.Concat("Error:", ex.Message, " Error Details: ", ex.InnerException, "Stack Trace", ex.StackTrace, $" Parameters: Origin: {flightOriginDestination.Origin} Destination: {flightOriginDestination.Destination}")));

                throw;
            }
        }


        public async Task<bool> ExistsJourneyFlight(int idJourney, int idFlight)
        {
            try
            {
                return await _AppDbContext.JourneyFlights.AnyAsync(x => x.IdJourney == idJourney && x.IdFlight == idFlight);
            }
            catch (Exception ex)
            {
                _Logger.LogError(Factory.GetError(string.Concat(nameof(JourneyRepository), " Repository at ", nameof(ExistsJourneyFlight)),
                  string.Concat("Error:", ex.Message, " Error Details: ", ex.InnerException, "Stack Trace", ex.StackTrace, $" Parameters: JourneyId {idJourney}  FlightId {idFlight}")));

                throw;
            }
        }
        public async Task<JourneyFlights> InsertJourneyFlight(Journeys journey, Flights flight,int index)
        {
            try
            {
               EntityEntry<JourneyFlights> journeyFlight = await _AppDbContext.JourneyFlights.AddAsync(new JourneyFlights { IdFlight = flight.IdFlight, IdJourney = journey.IdJourney, Index = index });
                bool saved = (await _AppDbContext.SaveChangesAsync() > 0);
                return saved ? journeyFlight.Entity:null;
            }
            catch (Exception ex)
            {
                _Logger.LogError(Factory.GetError(string.Concat(nameof(JourneyRepository), " Repository at ", nameof(InsertJourneyFlight)),
                  string.Concat("Error:", ex.Message, " Error Details: ", ex.InnerException, "Stack Trace", ex.StackTrace, $" Parameters: {typeof(Journeys)} {typeof(Flights)}")));

                throw;
            }
        }
        public async Task<Journeys> GetJourneyEntityByJourneyOriginDestination(OriginDestination originDestination)
        {
            try
            {
                return await (from flight in _AppDbContext.Flights
                              join journeyFlights in _AppDbContext.JourneyFlights on flight.IdFlight equals journeyFlights.IdFlight
                              join journey in _AppDbContext.Journeys on journeyFlights.IdJourney equals journey.IdJourney
                              where journey.Origin == originDestination.Origin && journey.Destination == originDestination.Destination
                              select journey).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _Logger.LogError(Factory.GetError(string.Concat(nameof(JourneyRepository), " Repository at ", nameof(GetJourneyEntityByJourneyOriginDestination)),
                  string.Concat("Error:", ex.Message, " Error Details: ", ex.InnerException, "Stack Trace", ex.StackTrace, $" Parameters: {typeof(OriginDestination)}")));
                throw;
            }
        }


        public async Task<Journeys> InsertJourney(Journeys journey)
        {
            try
            {
               
                EntityEntry<Journeys> flightIn = await _AppDbContext.Journeys.AddAsync(journey);
                bool saved = (await _AppDbContext.SaveChangesAsync() > 0);
                return saved ? flightIn.Entity : null;
            }
            catch (Exception ex)
            {
                _Logger.LogError(Factory.GetError(string.Concat(nameof(JourneyRepository), " Repository at ", nameof(InsertJourney)),
                  string.Concat("Error:", ex.Message, " Error Details: ", ex.InnerException, "Stack Trace", ex.StackTrace, $" Parameters: {typeof(Journeys)}")));

                throw;
            }
        }




    }
}
