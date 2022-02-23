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
        [InlineData("CTG", "MZL", 0)]
        [InlineData("CTG", "MZL", 1)]
        [InlineData("CTG", "MZL" ,2)]
        [InlineData("MZL", "MDE", 2)]
        [InlineData("CTG", "BOG", 2)]
        [InlineData("MZL", "MDE", 1)]
        [InlineData("CTG", "BOG", 1)]
        [InlineData("MZL", "MDE", 0)]
        [InlineData("CTG", "BOG", 0)]
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
