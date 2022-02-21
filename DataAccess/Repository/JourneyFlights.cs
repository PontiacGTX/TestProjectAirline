using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class JourneyFlights
    {
      
        public int Index { get; set; }
        public int IdJourney { get; set; }
        [ForeignKey("IdJourney")]
        public Journeys Journey { get; set; }

        public int IdFlight { get; set; }
        [ForeignKey("IdFlight")]
        public Flights Flights { get; set; }

    }
}
