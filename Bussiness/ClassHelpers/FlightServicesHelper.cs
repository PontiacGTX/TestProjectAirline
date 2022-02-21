using Bussiness;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bussiness.ClassHelper
{
    public static class FlightServicesHelper
    {
        public static void SetFoundFlightValues(this FlightsServices flightsServices, Flight flightFound, List< Flight> temp, ref bool found, ref List<List<Flight>> finalOriginDestionation, ref List<int> foundAt, int i)
        {
            temp.Add(flightFound);
            found = true;
            finalOriginDestionation.Add(new List<Flight>(temp));
            foundAt.Add(i);
        }
        public static void SetFoundFlightValues(this FlightsServices flightsServices,List<Flight> routes, ref List<List<Flight>> finalOriginDestionation)
        {
            finalOriginDestionation.Add(new List<Flight>(routes));
        }
    }
}
