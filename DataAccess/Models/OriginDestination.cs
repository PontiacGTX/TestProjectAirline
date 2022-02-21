using DataAccess.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class OriginDestination
    {
        public OriginDestination(JourneyRequestModel journeyRequestModel)
        {
            this.Origin = journeyRequestModel.Origin;
            this.Destination = journeyRequestModel.Destination;
        }
        public string Origin { get; set; }
        public string Destination { get; set; }
    }
}
