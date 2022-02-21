using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Flight
    {
        public Flight()
        {

        }
        public Flight(Flights flight)
        {
            Transport = new Transport { FlightCarrier = flight.Transport?.FlightCarrier, FlightNumber = flight.Transport?.FlightNumber };
            this.Price = flight.Price;
            this.Origin = flight.Origin;
            this.Destination = flight.Destination;
        }
        //[JsonPropertyName("transport")]
        public Transport Transport { get; set; } = new Transport();
        [JsonPropertyName("departureStation")]
        public string Origin { get; set; }
        [JsonPropertyName("arrivalStation")]
        public string Destination { get; set; }
        [JsonPropertyName("price")]
        public double Price { get; set; }

    }
}
