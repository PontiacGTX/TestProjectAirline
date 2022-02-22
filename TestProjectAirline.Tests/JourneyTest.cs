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
                Assert.Equal(await journeyTestServices.AreValidJourneys(journeys.ToList(), requestJourneyModel), new[] { true, true, true });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
