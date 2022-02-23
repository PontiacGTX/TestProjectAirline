using DataAccess;
using DataAccess.HelperClass;
using DataAccess.Models;
using DataAccess.Models.Requests;
using DataAccess.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            string basePath = Directory.GetCurrentDirectory();
            basePath = FIleHelper.CopyFileToDir(DirectoryHelper.GetNUpperDirectory(basePath,4),basePath, "appsettings.json", @"\TestProjectAirline\");
            if (File.Exists(basePath))
            {
              basePath = DirectoryHelper.GetNUpperDirectory(basePath, 1);
            }
            accessSettings = new AccessSettings(basePath);
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
