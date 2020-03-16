using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Enprecis.Users.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WingsOn.API.Attributes;
using WingsOn.API.Extensions;
using WingsOn.API.Web.Models;
using WingsOn.Data.Logic.Services.Interfaces;
using WingsOn.Domain;

namespace WingsOn.API.Controllers
{
    /// <summary>
    /// This controller is for Passengers of the WingsOn system,
    /// which connected with the flight entity and its appropriate bookings.
    /// </summary>
    [Route("api/Passengers")]
    [ApiController]
    public class PassengersController : Controller
    {
        private readonly IPassengerService _passengersService;
        private readonly ILogger<PassengersController> _logger;

        /// <summary>
        /// Initializes a new instance of the PassengersController class.
        /// Constructor for Passenger Controller.
        /// </summary>
        /// <param name="passengersService">The passengers service.</param>
        /// <param name="logger">The instance of a logger.</param>
        public PassengersController(
            IPassengerService passengersService,
            ILogger<PassengersController> logger)
        {
            _passengersService = passengersService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a passenger by its identifier.
        /// </summary>
        /// <param name="passengerId">The passenger identifier.</param>
        [Route("{passengerId:int}", Name = "GetPassenger")]
        [ResponseType(typeof(Person))]
        [HttpGet]
        public async Task<IActionResult> GetPassenger([FromRoute]int passengerId)
        {
            var passenger = await _passengersService.GetPersonEntityAsync(passengerId);
            if (passenger == null)
            {
                _logger.LogError($"Person '{passengerId}' does not exist");
                return NotFound();
            }
            return Ok(passenger);
        }

        /// <summary>
        /// Gets a collection of passengers by its gender.
        /// </summary>
        /// <param name="genderType">The gender type</param>
        /// <returns>The list of passengers by its gender.</returns>
        [Route("{genderType}/Gender", Name = "GetPassengersByGender")]
        [ResponseType(typeof(IEnumerable<Person>))]
        [HttpGet]
        public async Task<IActionResult> GetPassengersByGender([FromRoute]GenderType genderType)
        {
            var passengersByGender = await _passengersService.GetPeopleByGenderAsync(genderType);

            if (passengersByGender == null)
            {
                _logger.LogError($"Cannot find any passengers with transferred gender type '{genderType}'");
                return NotFound();
            }

            return Ok(passengersByGender);
        }

        /// <summary>
        /// Creates a new passenger using provided customerId and based on existing flight number.
        /// </summary>
        /// <param name="createPassengerRequest">The passenger to create.</param>
        /// <returns>Http Status Code</returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(int), HttpStatusCode.OK)]
        [ResponseType(typeof(int), HttpStatusCode.BadRequest)]
        [ResponseType(typeof(int), HttpStatusCode.ExpectationFailed)]
        public async Task<IActionResult> CreateAsync([FromBody]CreatePassengerRequest createPassengerRequest)
        {
            try
            {
                var resultCreating = await _passengersService
                    .CreatePersonWithBookingAsync(createPassengerRequest.ToPersonEntity(), 
                        createPassengerRequest.CustomerId, createPassengerRequest.FlightNumber);

                if (resultCreating.HasValue)
                {
                    if (resultCreating.Value)
                    {
                        return Ok();
                    }
                    else
                    {
                        _logger.LogError("Failed to create passenger. Invalid customer or flight data.");
                        return StatusCode(417, "There was a problem saving record. Please try again by providing valid customer or flight data.");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to create passenger. BadRequest.");
            }
            return BadRequest();
        }

        /// <summary>
        /// Update Passenger Address
        /// </summary>
        /// <param name="request">Update Passenger Request model</param>
        /// <returns>Http Status Code</returns>
        [HttpPut]
        [Route("")]
        [ResponseType(typeof(int), HttpStatusCode.OK)]
        [ResponseType(typeof(int), HttpStatusCode.BadRequest)]
        [ResponseType(typeof(int), HttpStatusCode.ExpectationFailed)]
        public async Task<IActionResult> UpdateAsync([FromBody]UpdatePassengerRequest request)
        {
            try
            {
                var updatedUser = await _passengersService.UpdatePersonAddressAsync(request.PersonId, request.Address);

                return Ok(updatedUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            return BadRequest();
        }
    }
}
