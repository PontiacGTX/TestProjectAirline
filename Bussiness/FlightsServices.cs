using Bussiness.ClassHelper;
using DataAccess;
using DataAccess.HelperClass;
using DataAccess.HelperClass.FactoryClass;
using DataAccess.Models;
using DataAccess.Models.Requests;
using DataAccess.Models.Responses;
using DataAccess.Repository;
using DataAccess.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bussiness
{
    public class FlightsServices
    {
        ILogger _Logger { get; }
        protected AccessSettings _AccessSettings { get;}
        protected NewShoreServices _NewShoreServices { get; }
        protected TransportRepository _TransportRepository { get; }
        protected FlightsRepository _FlightsRepository { get; }
        protected JourneyRepository _JourneyRepository { get; }
        public FlightsServices(ILogger logger, AccessSettings settingsAccess,NewShoreServices newShoreServices, 
            TransportRepository transportRepository, FlightsRepository flightsRepository, JourneyRepository journeyRepository)
        {
            _Logger = logger;
            _NewShoreServices = newShoreServices;
            _AccessSettings = settingsAccess;
            _TransportRepository = transportRepository;
            _FlightsRepository = flightsRepository;
            _JourneyRepository = journeyRepository;
        }
       
        public async Task<ResponseObject> GetFlights(JourneyRequestModel journeyRequest,PayloadRequestModel payload)
        {
            List<Flight> flights = null;
            var finalOriginDestionation = new List<List<Flight>>();
            Journey[] journeys = null;

            try
            {
                journeys = (await _JourneyRepository.GetJourneyByFlightDestination(new OriginDestination(journeyRequest))).ToArray() ;
            }
            catch 
            {
                //nothing to log
            }

            if (journeys is null or  { Length:0 })
            {
                try
                {
                    flights = (await _NewShoreServices.GetFights(payload.PayloadSize)).ToList();

                    var sellist = flights.Where(x => x.Origin == journeyRequest.Origin).Select(val => val).ToList();
                    List<Flight> temp = new List<Flight>();

                    List<int> foundAt = new List<int>();
                    foreach (var availableOrigin in sellist)
                    {
                        temp.Clear();
                        temp.Add(availableOrigin);
                       
                        bool found = false;
                        for (int i = 0; i < flights.Count; i++)
                        {
                            string destination = availableOrigin.Destination == journeyRequest.Origin ? availableOrigin.Origin : availableOrigin.Destination;
                            found = false;
                            if (flights[i].Origin == availableOrigin.Origin && flights[i].Destination == journeyRequest.Destination)
                            {
                                if (flights[i].Destination == journeyRequest.Destination
                                    && flights[i].Origin == journeyRequest.Origin)
                                {
                                    if (!foundAt.Any(x => x == i))
                                    {

                                        this.SetFoundFlightValues(flights[i], temp, ref found, ref finalOriginDestionation, ref foundAt, i);
                                        break;

                                    }
                                }
                            }

                            if (!found && flights.Count - 1 == i)
                            {
                                i = 0;
                                while (i < flights.Count)
                                {
                                    if (flights[i].Destination == journeyRequest.Destination && flights[i].Origin == availableOrigin.Destination)
                                    {
                                        if (!foundAt.Any(x => x == i))
                                        {

                                            this.SetFoundFlightValues(flights[i], temp, ref found, ref finalOriginDestionation, ref foundAt, i);
                                            break;
                                        }
                                    }

                                    if (!found && flights.Count - 1 == i)
                                    {
                                        var ListItem = (from inner in flights where inner.Origin == destination select inner).Where(x => x.Destination != journeyRequest.Origin).ToList();

                                        List<Flight> routes = new();
                                        destination = journeyRequest.Destination;
                                        bool foundFinalDest = false;
                                        Func<Flight, string, bool> WhereDestinationIsEitherOriginOrDestination = (x, destination) => x.Destination == destination || x.Origin == destination;
                                        if (ListItem.Any())
                                        {

                                            if (ListItem.Any(x => WhereDestinationIsEitherOriginOrDestination(x, destination)))
                                            {
                                                foundFinalDest = true;
                                                routes.AddRange(new[] { availableOrigin, ListItem.FirstOrDefault(x => WhereDestinationIsEitherOriginOrDestination(x, destination)) });
                                                break;
                                            }

                                            string lastDest = ListItem.First().Destination;

                                            foreach (var dest in ListItem)
                                            {
                                               
                                                lastDest = ListItem.First().Destination;
                                                var subDest = flights.Where(x => x.Origin == lastDest && x.Destination != journeyRequest.Origin).Where(x => ListItem.All(y => y.Origin != x.Destination)).ToList();
                                                if (subDest.Any(x => WhereDestinationIsEitherOriginOrDestination(x, destination)))
                                                {
                                                    foundFinalDest = true;
                                                    routes.Add(ListItem.FirstOrDefault(x => WhereDestinationIsEitherOriginOrDestination(x, destination)));

                                                    break;
                                                }
                                    
                                                for (int j = 0; j < subDest.Count; j++)
                                                {
                                                    if (WhereDestinationIsEitherOriginOrDestination(subDest[j], destination))
                                                    {
                                                        foundFinalDest = true;
                                                        routes.AddRange(new[] { availableOrigin, dest, subDest[j] });
                                                        break;
                                                    }
                                                    var InnerList = flights.Where(x => x.Origin == subDest[j].Destination && x.Destination != journeyRequest.Origin).ToList();
                                             
                                                    for (int k = 0; k < InnerList.Count; k++)
                                                    {
                                                        if (WhereDestinationIsEitherOriginOrDestination(InnerList[k], destination))
                                                        {
                                                            foundFinalDest = true;
                                                            routes.AddRange(new[] { availableOrigin, dest, subDest[j], InnerList[k] });
                                                            break;
                                                        }
                                                        //bool foundIt = InnerList[k].Destination == destination;
                                                       
                                                    }
                                                }
                                                if (foundFinalDest)
                                                {
                                                    this.SetFoundFlightValues(routes, ref finalOriginDestionation);
                                                    routes.Clear();
                                                    break;
                                                }
                                            }

                                            if (!found)
                                                break;
                                        }

                                    }
                                    i++;
                                }

                                if (found)
                                    break;

                            }
                        }
                    }
                    temp.Clear();

                    journeys = new Journey[finalOriginDestionation.Count];
                    int l = 0;
                    foreach (var travel in finalOriginDestionation)
                    {
                        journeys[l] = new Journey();
                        journeys[l].Destination = journeyRequest.Destination;
                        journeys[l].Origin = journeyRequest.Origin;
                        journeys[l].Flights = travel.Distinct().ToList();
                        journeys[l].Price = journeys[l].Flights.Sum(x => x.Price);
                        l++;
                    }

                    int index = 1;
                    foreach (var journey in journeys)
                    {
                        foreach (var flight in journey.Flights)
                        {

                            try
                            {
                                Transports transport = null;
                                Flights flightInserted = null;
                                Journeys journeysInsertedModel = null;
                                if (flight.Transport.ValuesAreNotNull)
                                {
                                    if (!(await _TransportRepository.TransportExist(new FlightCarrierFlightNumber(flight))))
                                        transport = await _TransportRepository.InsertTransport(new Transports(flight));
                                    else
                                    {
                                        transport = await _TransportRepository.GetTransportByFlightCarrierFlightNumber(new FlightCarrierFlightNumber(flight));
                                    }
                                }

                                if (transport is not null)
                                {
                                    Flights flightInsertModel = new Flights(flight);
                                    flightInsertModel.IdTransport = transport.IdTransport;
                                    flightInsertModel.Index = index;
                                    flightInserted = await _FlightsRepository.InsertFlight(flightInsertModel);
                                }
                                if (flightInserted is not null)
                                {
                                    if (!(await _JourneyRepository.ExistJourney(new OriginDestination(journeyRequest))))
                                    {
                                        var journeyObj = new Journeys(journey);
                                        journeyObj.Index = index;
                                        journeysInsertedModel = await _JourneyRepository.InsertJourney(journeyObj);
                                    }
                                    else
                                        journeysInsertedModel = await _JourneyRepository.GetJourneyEntityByJourneyOriginDestination(new OriginDestination(journeyRequest));

                                }

                                if (journeysInsertedModel is not null)
                                {
                                    if (journeysInsertedModel is not null && flightInserted is not null)
                                    {
                                        if (!(await _JourneyRepository.ExistsJourneyFlight(flightInserted.IdFlight, journeysInsertedModel.IdJourney)))
                                            await _JourneyRepository.InsertJourneyFlight(journeysInsertedModel, flightInserted, index);
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                _Logger.LogError(Factory.GetError(nameof(GetFlights),
                                  string.Concat(ex.Message, " Details: ", ex.InnerException, " StackTrace: ", ex.StackTrace), parameters: $"{journeyRequest}"));

                            }
                           
                        } 
                        index++;
                    }



                }
                catch (Exception ex)
                {
                    _Logger.LogError(Factory.GetError(String.Concat("Service: ", nameof(FlightsServices), " at", nameof(GetFlights)),
                        string.Concat(ex.Message, $" Details:", ex.InnerException, " StackTrace: ", ex.StackTrace), parameters: $"{nameof(payload.PayloadSize)} {payload.PayloadSize}"));
                    throw;
                }
            }


            return Factory.GetResponse<ResponseObject>(journeys);
        }
        public async Task<ResponseObject> GetPayload(int payload)
        {
            object response = null;
            try
            {
                response = payload switch
                {
                    (int)PayloadSizeEnum.Small => await _NewShoreServices.GetFights(_AccessSettings.SmallPayloadSize),
                    (int)PayloadSizeEnum.Medium => await _NewShoreServices.GetFights(_AccessSettings.MediumPayloadSize),
                    (int)PayloadSizeEnum.Large => await _NewShoreServices.GetFights(_AccessSettings.LargePayloadSize),
                    (int)PayloadSizeEnum.All => new Dictionary<int, IList<Flight>>()
                    {
                        {(int)PayloadSizeEnum.Small,  await _NewShoreServices.GetFights(_AccessSettings.SmallPayloadSize) },
                        {(int)PayloadSizeEnum.Medium, await _NewShoreServices.GetFights(_AccessSettings.MediumPayloadSize) },
                        {(int)PayloadSizeEnum.Large , await _NewShoreServices.GetFights(_AccessSettings.LargePayloadSize) }
                    },
                    _ => Array.Empty<object>(),
                };
            }
            catch (Exception ex)
            {
                _Logger.LogError(Factory.GetError(nameof(GetPayload),
                    string.Concat(ex.Message," Details: ",ex.InnerException," StackTrace: ", ex.StackTrace),parameters:$"{payload}"));
                throw;
            }

           return Factory.GetResponse<ResponseObject>(response);
        }


    }
}
