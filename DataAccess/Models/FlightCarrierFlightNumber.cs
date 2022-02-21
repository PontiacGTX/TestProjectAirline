using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class FlightCarrierFlightNumber
    {
        public FlightCarrierFlightNumber(Flight flight)
        {
            this.FlightCarrier = flight.Transport.FlightCarrier;
            this.FlightNumber = flight.Transport.FlightNumber;
        }
        public string FlightCarrier { get; set; }
        public string FlightNumber { get; set; }
    }
}
