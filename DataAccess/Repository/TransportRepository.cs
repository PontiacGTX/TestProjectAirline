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
    public class TransportRepository
    {
        AccessSettings _AccessSettings { get; }
        protected ILogger _Logger { get; }
        protected AppDbContext _AppDbContext { get; }
        public TransportRepository(AppDbContext ctx, ILogger logger, AccessSettings accessSettings)
        {
            _AccessSettings = accessSettings;
            _Logger = logger;
            _AppDbContext = ctx;
        }

        public async Task<bool> TransportExist(FlightCarrierFlightNumber flightCarrierFlightNumber)
        {
            try
            {
                return await _AppDbContext.Transports.AnyAsync(x => x.FlightCarrier == flightCarrierFlightNumber.FlightCarrier
                && x.FlightNumber == flightCarrierFlightNumber.FlightNumber);
            }
            catch (Exception ex)
            {
                _Logger.LogError(Factory.GetError(string.Concat(nameof(TransportRepository), " Repository at ", nameof(TransportExist)),
                string.Concat("Error:", ex.Message, " Error Details: ", ex.InnerException, "Stack Trace", ex.StackTrace, $" Parameters: FlightCarrier {flightCarrierFlightNumber.FlightCarrier} FlightNumber: {flightCarrierFlightNumber.FlightNumber}")));

                throw;
            }
        }
        public async Task<Transports> GetTransportByFlightCarrierFlightNumber(FlightCarrierFlightNumber flightCarrierFlightNumber)
        {
            try
            {
                return await _AppDbContext.Transports.FirstOrDefaultAsync(x => x.FlightCarrier == flightCarrierFlightNumber.FlightCarrier
                && x.FlightNumber == flightCarrierFlightNumber.FlightNumber);
            }
            catch (Exception ex)
            {
                _Logger.LogError(Factory.GetError(string.Concat(nameof(TransportRepository), " Repository at ", nameof(GetTransportByFlightCarrierFlightNumber)),
                string.Concat("Error:", ex.Message, " Error Details: ", ex.InnerException, "Stack Trace", ex.StackTrace, $" Parameters: FlightCarrier {flightCarrierFlightNumber.FlightCarrier} FlightNumber: {flightCarrierFlightNumber.FlightNumber}")));

                throw;
            }
        }
        public async Task<Transports> InsertTransport(Transports transport)
        {
            try
            {
               
                EntityEntry<Transports> transportIn = await _AppDbContext.Transports.AddAsync(transport);
                bool saved = (await _AppDbContext.SaveChangesAsync() > 0);
                return saved ? transportIn.Entity : null;
            }
            catch (Exception ex)
            {
                _Logger.LogError(Factory.GetError(string.Concat(nameof(TransportRepository), " Repository at ", nameof(InsertTransport)),
                  string.Concat("Error:", ex.Message, " Error Details: ", ex.InnerException, "Stack Trace", ex.StackTrace, $" Parameters: {typeof(Journeys)}")));

                throw;
            }
        }
    }
}
