using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    
    public class Transports
    {
        public Transports()
        {

        }
        public Transports(Flight flight)
        {
            this.FlightCarrier = flight.Transport?.FlightCarrier;
            this.FlightNumber = flight.Transport?.FlightNumber;
        }
        [Key]
        public int IdTransport { get; set; }
        public string FlightCarrier { get; set; }
        public string FlightNumber { get; set; }
        public ICollection<Flights> Flights { get; set; }
    }
}
