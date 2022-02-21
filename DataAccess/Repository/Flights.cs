using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class Flights
    {
        public Flights()
        {

        }
        public Flights(Flight flight)
        {
            this.Destination = flight.Destination;
            this.Origin = flight.Origin;
            this.Price = flight.Price;
          
        }
        [Key]
        public int IdFlight { get; set; }
        public ICollection<JourneyFlights> JourneyFlights { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
        public int Index { get; set; }
        public int IdTransport { get; set; }
        [ForeignKey("IdTransport")]
        public virtual Transports Transport { get; set; }
    }
}
