using DataAccess.Converter;
using DataAccess.Exceptions;
using DataAccess.HelperClass.FactoryClass;
using DataAccess.Models;
using DataAccess.Models.Requests;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class NewShoreServices
    {

        protected HttpClient _HttpClient { get; set; }
        protected AccessSettings _SettingsAccess { get; }
        protected ILogger _Logger { get; }
        string BaseUrl { get; set; }
        string RequestUrl(string parameters) => $"{BaseUrl}{parameters}";
        public NewShoreServices(ILogger logger,HttpClient httpClient, AccessSettings settingsAccess)
        {
            _HttpClient = httpClient;
            _SettingsAccess = settingsAccess;
            BaseUrl = _SettingsAccess.APIBaseUrl;
            _Logger = logger;
        }

        public List<Flight> GetFlights(string txt)
            => JsonConvert.DeserializeObject<IEnumerable<Flight>>(txt, new FlightResponseConverter()).ToList();

        /// <summary>
        /// Gets payload by Size
        /// </summary>
        /// <param name="payloadSize"></param>
        /// <returns></returns>
        public async Task<IList<Flight>> GetFights(int payloadSize)
        {
            HttpResponseMessage responseMessage = null;
           List <Flight> flights = new List<Flight>();
            try
            {
                string url = RequestUrl($"{payloadSize}");
                _HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                 responseMessage = await _HttpClient.GetAsync(url);
                if(responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new ForeignApiErrorException("Api responded with error",(int)responseMessage.StatusCode);
                }

                
                 flights = JsonConvert.DeserializeObject<IEnumerable<Flight>>(await responseMessage.Content.ReadAsStringAsync(), new FlightResponseConverter()).ToList();
                
                return flights;
            }
            catch (Exception ex)
            {
                string reason = responseMessage is not null ? responseMessage.ReasonPhrase:null;
                _Logger.LogError((Factory.GetError(string.Concat("Service: ",nameof(NewShoreServices)," at ",nameof(GetFights)),
                    string.Concat(ex.Message, $" Details: {reason}", ex.InnerException, " StackTrace: ", ex.StackTrace), parameters: $"{payloadSize}")));
                throw;
            }
        }

        public async Task<IList<Flight>> GetFights(string flights)
            => JsonConvert.DeserializeObject<IEnumerable<Flight>>(flights, new FlightResponseConverter()).ToList();

           
    }
}
