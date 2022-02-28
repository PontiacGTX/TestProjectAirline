using Business.ClassHelper;
using DataAccess;
using DataAccess.HelperClass;
using DataAccess.HelperClass.FactoryClass;
using DataAccess.Models;
using DataAccess.Models.Requests;
using DataAccess.Models.Responses;
using DataAccess.Repository;
using DataAccess.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.GraphModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Business
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
        public async Task<List<Flight>> GetFlights(JourneyRequestModel journeyRequestModel, Flight SeletedFlightOrigin, Flight SelectedFlightDestionation, List<Flight> selecectedFlightList, List<Flight> flights, Flight skip, int currentSelect = 0, List<Flight> selectedPreviousList = null, int remaningToCheck = 0)
        {

           
            
            if(SelectedFlightDestionation is not null)
            if (!selecectedFlightList.Any(x => x.Origin == SelectedFlightDestionation?.Origin && x.Destination == SelectedFlightDestionation?.Destination))
                 selecectedFlightList.Add(SelectedFlightDestionation);


            if (SelectedFlightDestionation is null && remaningToCheck ==0)
                return selecectedFlightList;
         

            if (SelectedFlightDestionation.Destination == journeyRequestModel.Destination || SelectedFlightDestionation.Origin ==journeyRequestModel.Destination)
                return selecectedFlightList;

            var selectedPrevDest = SelectedFlightDestionation;
            Flight selectedDest = null;
            if (skip is null)
                selectedDest = flights.FirstOrDefault(x => x.Origin == selectedPrevDest.Destination && x.Destination != SelectedFlightDestionation.Origin);
            else
            {
                var items = flights.Where(x => x.Origin == selectedPrevDest.Destination && x.Destination != SelectedFlightDestionation.Origin && x.Destination != skip.Origin && x.Destination != journeyRequestModel.Origin  && selectedPreviousList.All(y=>y.Origin !=x.Destination)).ToArray();
                selectedDest = items.Any() ? items[currentSelect] : null;
                if(selectedDest is not null)
                selectedPreviousList.Add(selectedDest);

                remaningToCheck = items.Count();
            }
            return await GetFlights(journeyRequestModel, selectedPrevDest, selectedDest, selecectedFlightList, flights,skip, currentSelect, selectedPreviousList);
        }
        public async Task<ResponseObject> GetFlightRoutes(JourneyRequestModel journeyRequestModel, PayloadRequestModel payload)
        {
            List<Flight> flights = null;
            Journey[] journeys = null;
            
            if (!this.TryGetJourneys(ref journeys, _JourneyRepository, journeyRequestModel))
            {

                SortedDictionary<Flight, Dictionary<string, List<Flight>>> dic = new SortedDictionary<Flight, Dictionary<string, List<Flight>>>();
                List<Flight> flightList = (await _NewShoreServices.GetFights(payload.PayloadSize)).ToList();
                List<Flight> selectedflightsWithJourneysOrigin = flightList.Where(x => x.Origin == journeyRequestModel.Origin).ToList();


                List<Flight> selectedFlights = new List<Flight>();
                List<Flight> previousRoutes = new List<Flight>();
                List<List<Flight>> availableFlights = new List<List<Flight>>();
                bool checkNewFlight = false;
                for (int i= 0;i<selectedflightsWithJourneysOrigin.Count;i++)
                {
                    selectedFlights = await GetFlights(journeyRequestModel, null, selectedflightsWithJourneysOrigin[i], selectedFlights,flightList,null);
                    if (selectedFlights.Any())
                    {
                        int tries = flightList.Count(x=>x.Origin == selectedFlights.Last().Origin);
                        int addedAt = 0;
                        int ctr = 0;
                        bool isFirstRun = true;
                        List<Flight> innerPreviousFlights = new List<Flight>();
                        List<Flight> remaningCheck = new List<Flight>();
                        Flight lastOrign = null;
                        int indexLastOrigin = 0;
                        while (!selectedFlights.Any(x => x.Destination == journeyRequestModel.Destination) || ctr< tries)
                        {
                            
                            if (selectedFlights.Any(x => x.Destination == journeyRequestModel.Destination))
                            {
                                availableFlights.Add(new List<Flight>(selectedFlights));
                                break;
                            }

                            List<Flight> flight1 = null;
                            Flight lastSelected = selectedFlights.Last();
                            if ( selectedFlights is { Count:>0})
                            {
                                addedAt = isFirstRun ? 2: selectedFlights.Count();
                                if(isFirstRun)
                                {
                                    lastOrign = selectedFlights[selectedFlights.Count - 1];
                                    remaningCheck = flightList.Where(x => x.Origin == lastOrign.Origin && !previousRoutes.Any(y => x.Destination == y.Destination) && x.Destination != selectedflightsWithJourneysOrigin[i].Origin).ToList();//[ctr]
                                    selectedFlights = selectedFlights.Take(addedAt).ToList();
                                    lastOrign = selectedFlights.Last();
                                    isFirstRun = false;
                                }
                                string lastOrigin =selectedFlights[selectedFlights.Count-1].Origin;
                                
                                flight1 = flightList.Where(x =>  x.Origin == lastOrigin && !previousRoutes.Any(y => x.Destination == y.Destination) && x.Destination!=selectedflightsWithJourneysOrigin[i].Origin).ToList();//[ctr]
                                if(flight1.Count>0)
                                    lastSelected = flight1.Last();



                                if (flight1.Count ==0)
                                {
                                    lastOrign = remaningCheck[indexLastOrigin];
                                    flight1 = flightList.Where(x => x.Origin == lastOrign.Origin && x.Destination != lastOrign.Origin && !previousRoutes.Any(y => x.Destination == y.Destination) && x.Destination != selectedflightsWithJourneysOrigin[i].Origin).ToList();//[ctr]
                                    lastSelected = flight1[indexLastOrigin];
                                    indexLastOrigin++;
                                    
                                }
                             
                                previousRoutes.Add(lastSelected);
                                isFirstRun = false;
                            }
                            selectedFlights = await GetFlights(journeyRequestModel, null, lastSelected, selectedFlights, flightList, lastSelected, ctr, innerPreviousFlights);
                            if (selectedFlights.Any(x => x.Destination == journeyRequestModel.Destination))
                            {
                                availableFlights.Add(new List<Flight>(selectedFlights));
                                break;
                            }
                            

                            if (selectedFlights is not { Count:0 })
                            {
                                checkNewFlight = true;
                                remaningCheck = flightList.Where(x => x.Origin == lastOrign.Origin && !previousRoutes.Any(y => x.Destination == y.Destination) && x.Destination != selectedflightsWithJourneysOrigin[i].Origin).ToList();//[ctr]

                                selectedFlights = selectedFlights.Take(addedAt).ToList();
                            }
                            
                            ctr++;
                        }
                        innerPreviousFlights.Clear();
                    }

                   
                    selectedFlights.Clear();
                }

                journeys = new Journey[availableFlights.Count];
                int l = 0;
                foreach (var travel in availableFlights)
                {
                    journeys[l] = new Journey();
                    journeys[l].Destination = journeyRequestModel.Destination;
                    journeys[l].Origin = journeyRequestModel.Origin;
                    journeys[l].Flights = travel.Distinct().ToList();
                    journeys[l].Price = journeys[l].Flights.Sum(x => x.Price);
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
                                if (!(await _JourneyRepository.ExistJourney(new OriginDestination(journeyRequestModel))))
                                {
                                    var journeyObj = new Journeys(journey);
                                    journeyObj.Index = index;
                                    journeysInsertedModel = await _JourneyRepository.InsertJourney(journeyObj);
                                }
                                else
                                    journeysInsertedModel = await _JourneyRepository.GetJourneyEntityByJourneyOriginDestination(new OriginDestination(journeyRequestModel));

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
                              string.Concat(ex.Message, " Details: ", ex.InnerException, " StackTrace: ", ex.StackTrace), parameters: $"{journeyRequestModel}"));

                        }

                    }
                    index++;
                }
            }
            return Factory.GetResponse<ResponseObject>(journeys);
        }
        public async Task<ResponseObject> GetFlight(JourneyRequestModel journeyRequest,PayloadRequestModel payload)
        {
            List<Flight> flights = null;
            var finalOriginDestionation = new Dictionary<int,List<Flight>>();
            Journey[] journeys = null;
       
            

            if (! this.TryGetJourneys(ref journeys, _JourneyRepository, journeyRequest))
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
                                    if (!finalOriginDestionation.ContainsKey(i))
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
                                        if (!finalOriginDestionation.ContainsKey(i))
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
                                        
                                        if (ListItem.Any())
                                        {
                                            this.GetRemainingFlightList(journeyRequest,availableOrigin,destination,ListItem,ref flights,ref foundFinalDest,ref routes);
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
                        journeys[l].Flights = travel.Value.Distinct().ToList();
                        journeys[l].Price = journeys[l].Flights.Sum(x => x.Price);
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
            IList<Flight> response = null;
            try
            {
                response = payload switch
                {
                    (int)PayloadSizeEnum.Small => await _NewShoreServices.GetFights(_AccessSettings.SmallPayloadSize),
                    (int)PayloadSizeEnum.Medium => await _NewShoreServices.GetFights(_AccessSettings.MediumPayloadSize),
                    (int)PayloadSizeEnum.Large => await _NewShoreServices.GetFights(_AccessSettings.LargePayloadSize),
                    _ => Array.Empty<Flight>(),
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
