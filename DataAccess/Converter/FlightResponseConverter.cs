using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DataAccess.Models;
using Newtonsoft.Json.Linq;

namespace DataAccess.Converter
{
    internal class FlightResponseConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType) => true;

        public override List<Flight> ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;


            List<Flight> flights = new List<Flight>();

            var jObject = JArray.Load(reader);
            JArray flightsArr = jObject;

            Flight flight = null;
            foreach (var flightObj in flightsArr)
            {
                flight = new Flight();
                flight.Origin = flightObj["departureStation"].ToString();
                flight.Destination = flightObj["arrivalStation"].ToString();
                double.TryParse(flightObj["price"].ToString(), out double price);
                flight.Price = price;
                flight.Transport.FlightCarrier = flightObj["flightCarrier"].ToString();
                flight.Transport.FlightNumber = flightObj["flightNumber"].ToString();
                flights.Add(flight);
            }

            return flights;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
