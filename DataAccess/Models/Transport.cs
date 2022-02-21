using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Transport
    {
        [JsonPropertyName("flightCarrier")]
        public string FlightCarrier { get; set; }
        [JsonPropertyName("flightNumber")]
        public string FlightNumber { get; set; }

        public bool ValuesAreNotNull =>
            !string.IsNullOrEmpty(FlightNumber) && !string.IsNullOrEmpty(FlightCarrier);
    }
}
