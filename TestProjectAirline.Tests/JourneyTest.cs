using DataAccess.Models;
using DataAccess.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProjectAirline.Tests.Services;
using TestServices.Services;
using Xunit;

namespace TestProjectAirline.Tests
{
    public class JourneyTest
    {

        [Theory]
        [InlineData("MZL","MDE",2)]
        public async Task AreJorneysValid(string origin,string destination, int payload)
        {
            try
            {
                var requestJourneyModel = new JourneyRequestModel
                {
                    Origin = origin,
                    Destination = destination
                };
                JourneyServices services = new JourneyServices();
                IEnumerable<Journey> journeys = await services.GetJourney(requestJourneyModel, payload);
                JourneyTestServices journeyTestServices = new();
                int count = journeys.Count();
                List<bool> expected = new List<bool>(Enumerable.Range(1, count).Select(x => true));
                var result = (await journeyTestServices.AreValidJourneys(journeys.ToList(), requestJourneyModel)).ToList();
                Assert.Equal(result, expected);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
