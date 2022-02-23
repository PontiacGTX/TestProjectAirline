using DataAccess;
using DataAccess.Models;
using DataAccess.Models.Requests;
using DataAccess.Models.Responses;
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
                    var url = Request(accessSettings.ApiUrl, accessSettings.ApiEndPointGetJourneys, $"{payloadSize}");
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    string json = JsonConvert.SerializeObject(request);
                    HttpResponseMessage responseMessage = await  httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                    responseMessage.EnsureSuccessStatusCode();
                    if (responseMessage.IsSuccessStatusCode)
                    {

                      ResponseObject response =   JsonConvert.DeserializeObject<ResponseObject>(await responseMessage.Content.ReadAsStringAsync());    
                      journeys = JsonConvert.DeserializeObject<IEnumerable <Journey>>(response.Data.ToString()) ;
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
