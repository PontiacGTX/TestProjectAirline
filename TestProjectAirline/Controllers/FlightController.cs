using Business;
using DataAccess.HelperClass;
using DataAccess.HelperClass.FactoryClass;
using DataAccess.Models;
using DataAccess.Models.Requests;
using DataAccess.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TestProjectAirline.Controllers
{
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private ILogger<FlightController> _logger { get; }
        private FlightsServices FlightsServices { get; }

        public FlightController(ILogger<FlightController> logger, FlightsServices flightsServices)
        {
            FlightsServices = flightsServices;
            _logger = logger;
        }

        [HttpPost("GetFlights")]
        public async Task<IActionResult> GetFlights([FromBody] JourneyRequestModel journey, PayloadRequestModel payload)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(await FlightsServices.GetFlightRoutes(journey, payload));
                }
                catch (Exception ex)
                {
                    string errorInformation = ex.Message;
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        Factory.GetResponse<ErrorResponseObject>(Factory.GetStringError(ErrorStringEnum.FailedRequest),
                        StatusCodes.Status500InternalServerError, Factory.GetStringError(ErrorStringEnum.InternalServerError) + $" {errorInformation}"));
                }
            }

            string invalidFields = ModelStateValidatorHelper.GetModelErrors(ModelState);

            return BadRequest(Factory.GetResponse<ErrorResponseObject>(Factory.GetStringError(ErrorStringEnum.BadRequestError) + invalidFields,
                StatusCodes.Status400BadRequest,
                 Factory.GetStringError(ErrorStringEnum.BadRequestError)));
        }




        [HttpPost("FlightIndex")]
        public async Task<IActionResult> FlightIndex([FromBody] PayloadRequestModel payloadRequest)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(await FlightsServices.GetPayload(payloadRequest.PayloadSize));
                }
                catch (Exception ex)
                {
                    string errorInformation = ex.Message;
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        Factory.GetResponse<ErrorResponseObject>(Factory.GetStringError(ErrorStringEnum.FailedRequest),
                        StatusCodes.Status500InternalServerError, Factory.GetStringError(ErrorStringEnum.InternalServerError) + $" {errorInformation}"));
                }
            }

            return BadRequest(Factory.GetResponse<ErrorResponseObject>(Factory.GetStringError(ErrorStringEnum.BadRequestError),
                StatusCodes.Status400BadRequest,
                Factory.GetStringError(ErrorStringEnum.BadRequestError)));
        }



    }
}
