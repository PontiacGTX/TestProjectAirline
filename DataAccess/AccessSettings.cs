using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;

namespace DataAccess
{
    public class AccessSettings
    {
        IConfiguration _Configuration { get; }
        public AccessSettings()
        {
            var builder = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _Configuration = builder.Build();
        }
        public AccessSettings(string path)
        {
            var builder = new ConfigurationBuilder()
                     .SetBasePath(path)
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _Configuration = builder.Build();
        }
        public string APIBaseUrl => _Configuration["BaseAPI:URL"];
        public int SmallPayloadSize => int.Parse(_Configuration["PayloadSize:Small"]);
        public int MediumPayloadSize => int.Parse(_Configuration["PayloadSize:Medium"]);
        public int LargePayloadSize => int.Parse(_Configuration["PayloadSize:Large"]);
        public string Flights => _Configuration["FlightList:Flights"];
        public string ConString => _Configuration["ConnectionStrings:DbConnection"];
        public string ApiUrl => _Configuration["TestProjectAPI:Uri"];
        public string ApiEndPointGetJourneys => _Configuration["TestProjectAPI:GetJourneysEndPoint"];
    }
}
