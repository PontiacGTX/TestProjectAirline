using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.HelperClass
{
    public static class JourneyClassHelper
    {
        public static  IList<IList<Journey>> OrganizeJourneys(this IList<DataAccess.Repository.Journeys> listJourneys)
        {

            var journeys = listJourneys.Select(x => x.JourneyFlights.Where(y => y.Flights.Index == x.Index).ToList()).ToList().Select((journey,idx)=>new {journey,idx }).ToDictionary(x=>x.idx);

            //foreach(var j in journeys)
            //{
            //    foreach (var journeyObj  in j.Value as List<Journey>)
            //    {

            //    }
            //}
            return null;
        }
    }
}
