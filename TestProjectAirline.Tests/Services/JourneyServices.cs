using DataAccess;
using DataAccess.Models;
using DataAccess.Models.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectAirline.Tests.Services
{
    public class JourneyServices
    {
        protected AccessSettings accessSettings { get; }
        protected HttpClient httpClient { get; set; }
        string Request(params string[] arg) => string.Concat(arg);
        public JourneyServices()
        {
            accessSettings = new AccessSettings();
        }
        public async Task<IEnumerable<Journey>> GetJourney(JourneyRequestModel request,int payloadSize)
        {
            IEnumerable<Journey> journeys = new List<Journey>();
            try
            {
                using (httpClient =  new HttpClient())
                {
                    var url = Request(accessSettings.APIBaseUrl, accessSettings.ApiEndPointGetJourneys, $"{payloadSize}");
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    string json = JsonConvert.SerializeObject(request);
                    HttpResponseMessage responseMessage = await  httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                    responseMessage.EnsureSuccessStatusCode();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        journeys = JsonConvert.DeserializeObject<IEnumerable<Journey>>(await responseMessage.Content.ReadAsStringAsync());    
                    }

                }
                return journeys;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
