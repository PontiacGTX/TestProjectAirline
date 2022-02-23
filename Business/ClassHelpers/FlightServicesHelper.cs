using Business;
using DataAccess.HelperClass.FactoryClass;
using DataAccess.Models;
using DataAccess.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.ClassHelper
{
    public static class FlightServicesHelper
    {
        public static void SetFoundFlightValues(this FlightsServices flightsServices, Flight flightFound, List<Flight> temp, ref bool found, ref List<List<Flight>> finalOriginDestionation, ref List<int> foundAt, int i)
        {
            temp.Add(flightFound);
            found = true;
            finalOriginDestionation.Add(new List<Flight>(temp));
            foundAt.Add(i);
        }
        public static void SetFoundFlightValues(this FlightsServices flightsServices, Flight flightFound, List<Flight> temp, ref bool found, ref Dictionary<int, List<Flight>> finalOriginDestionation, ref List<int> foundAt, int i)
        {
            temp.Add(flightFound);
            foundAt.Add(i);
            if (finalOriginDestionation.TryGetValue(i, out List<Flight> flights))
            {
                flights.AddRange(new List<Flight>(temp));
                finalOriginDestionation[i] = flights;
            }
            else
            {
                List<Flight> newFlights = new List<Flight>(temp);
                finalOriginDestionation[i] = newFlights;
            }
        }
        public static void SetFoundFlightValues(this FlightsServices flightsServices, List<Flight> routes, ref List<List<Flight>> finalOriginDestionation)
        {
            finalOriginDestionation.Add(new List<Flight>(routes));
        }
        public static void SetFoundFlightValues(this FlightsServices flightsServices, List<Flight> routes, ref Dictionary<int, List<Flight>> finalOriginDestionation)
        {
            finalOriginDestionation.Add(finalOriginDestionation.Keys.Count + 1, new List<Flight>(routes));
        }
        public static bool GetRemainingFlightList(this FlightsServices flightServices, JourneyRequestModel journeyRequest, Flight availableOrigin, string destination, List<Flight> remainingFlightList,
            ref List<Flight> flightsList, ref bool foundFinalDest, ref List<Flight> routes)
        {
            Func<Flight, string, bool> WhereDestinationIsEitherOriginOrDestination = (x, destination) => x.Destination == destination || x.Origin == destination;

            if (remainingFlightList.Any(x => WhereDestinationIsEitherOriginOrDestination(x, destination)))
            {
                foundFinalDest = true;
                routes.AddRange(new[] { availableOrigin, remainingFlightList.FirstOrDefault(x => WhereDestinationIsEitherOriginOrDestination(x, destination)) });
                return foundFinalDest;
            }
             string lastDest = remainingFlightList.First().Destination;
            foreach (var dest in remainingFlightList)
            {

                
                var subDest = flightsList.Where(x => x.Origin == lastDest && x.Destination != journeyRequest.Origin).Where(x => remainingFlightList.All(y => y.Origin != x.Destination)).ToList();
                if (subDest.Any(x => WhereDestinationIsEitherOriginOrDestination(x, destination)))
                {
                    foundFinalDest = true;
                    routes.Add(remainingFlightList.FirstOrDefault(x => WhereDestinationIsEitherOriginOrDestination(x, destination)));

                    return foundFinalDest;
                }

                for (int j = 0; j < subDest.Count; j++)
                {
                    if (WhereDestinationIsEitherOriginOrDestination(subDest[j], destination))
                    {
                        foundFinalDest = true;
                        routes.AddRange(new[] { availableOrigin, dest, subDest[j] });
                        return foundFinalDest;
                    }
                    var InnerList = flightsList.Where(x => x.Origin == subDest[j].Destination && x.Destination != journeyRequest.Origin).ToList();

                    for (int k = 0; k < InnerList.Count; k++)
                    {
                        if (WhereDestinationIsEitherOriginOrDestination(InnerList[k], destination))
                        {
                            foundFinalDest = true;
                            routes.AddRange(new[] { availableOrigin, dest, subDest[j], InnerList[k] });
                            return foundFinalDest;
                        }

                    }
                }

            
            }return foundFinalDest;
        }
    }
}
