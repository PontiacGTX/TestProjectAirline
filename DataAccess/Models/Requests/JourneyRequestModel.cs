using DataAccess.HelperClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Requests
{
    public class JourneyRequestModel
    {
        [StringValidation(Pattern ="[a-zñáéíóúA-ZÑÁÉÍÓÚ]")]
        [JsonProperty("origin")]
        public string Origin { get; set; }
        [JsonProperty("destination")]
        public string Destination { get; set; }
    }
}
