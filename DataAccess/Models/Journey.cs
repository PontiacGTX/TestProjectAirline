using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Journey
    {
        public Journey()
        {

        }
        public Journey(Journeys journey)
        {
            this.Destination = journey.Destination;
            this.Origin = journey.Origin;
            this.Price = journey.Price;
           
        }
        [JsonPropertyName("flights")]
        public List<Flight> Flights { get; set; } = new List<Flight>();
        [JsonPropertyName("origin")]
        public string Origin { get; set; }
        [JsonPropertyName("destination")]
        public string Destination { get; set; }
        [JsonPropertyName("price")]
        public double Price { get; set; }
    }
}
