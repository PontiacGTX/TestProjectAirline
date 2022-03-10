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
    public class Journeys
    {
        public Journeys()
        {

        }
        public Journeys(Journey journey)
        {
            this.Price = journey.Price;
            this.Origin = journey.Origin;
            this.Destination = journey.Destination;
        }

        [Key]
        public int IdJourney { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
        public ICollection<JourneyFlights> JourneyFlights { get; set; }
    }
}
