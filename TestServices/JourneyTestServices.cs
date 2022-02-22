using DataAccess.Models;
using DataAccess.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestServices.Services
{
    public class JourneyTestServices
    {
        public async Task<IEnumerable<bool>> AreValidJourneys(IList<Journey> journeys,JourneyRequestModel journeyRequestModel )
        {
            int itemCount = journeys.Count;
            List<(bool, bool)> validations = new List<(bool, bool)>(itemCount);
            bool foundFirstOrigin = false;
            bool checkedFoundOrigin = false;
            for (int i = 0; i < journeys.Count; i++)
            {
                (bool, bool) checks = (false, false);
                foreach (var flight in journeys[i].Flights)
                {
                    
                    if (flight.Origin == journeyRequestModel.Origin)
                    {
                        if(!checkedFoundOrigin)
                             foundFirstOrigin = true;

                        checks.Item1 = true;
                        checkedFoundOrigin = true;
                    }

                    if (flight.Destination == journeyRequestModel.Destination)
                    {

                        checks.Item1 = true;
                    }


                }

                if (!foundFirstOrigin)
                    checks = (false, false);


                validations.Add(checks);
                foundFirstOrigin = false;
            }


            return validations.Select(x => x.Item1 && x.Item2);
        }
    }
}
