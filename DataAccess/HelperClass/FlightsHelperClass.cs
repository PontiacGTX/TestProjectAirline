using DataAccess.Models;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.HelperClass
{
    public static class FlightsHelperClass
    {
        public static Flight ToFlight(this Flights flight)
        {
            Flight flightResult = null;
            if(flight is not null)
            {
                flightResult.Destination = flight.Destination;
                flightResult.Origin = flight.Origin;
                flightResult.Price = flight.Price;
                if(flight.Transport is not null)
                {
                    flightResult.Transport = new Transport
                    {
                        FlightCarrier = flight.Transport?.FlightCarrier,
                        FlightNumber = flight.Transport?.FlightNumber
                    };
                }
            }

            return flightResult;
        }
    }
}
