using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WingsOn.API.Attributes;
using WingsOn.Data.Logic.Services.Interfaces;
using WingsOn.Domain;

namespace WingsOn.API.Controllers
{
    /// <summary>
    /// This controller is for Bookings of the WingsOn system,
    /// which connected with the flight entity and its appropriate passengers.
    /// </summary>
    [Route("api/Bookings")]
    [ApiController]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingsService;
        private readonly ILogger<PassengersController> _logger;

        /// <summary>
        /// Initializes a new instance of the BookingsController class.
        /// Constructor for Booking Controller.
        /// </summary>
        /// <param name="bookingsService">The bookings service.</param>
        /// <param name="logger">The instance of a logger.</param>
        public BookingsController(
            IBookingService bookingsService,
            ILogger<PassengersController> logger)
        {
            _bookingsService = bookingsService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a collection of passengers that belong to a booking by passed flight number.
        /// </summary>
        /// <param name="flightNumber">The flight number</param>
        /// <returns>The list of passengers for the given flight number and it's bookings.</returns>
        [Route("{flightNumber}/Passengers", Name = "GetPassengersByFlightNumberOfBooking")]
        [ResponseType(typeof(IEnumerable<Person>))]
        [HttpGet]
        public async Task<IActionResult> GetPassengersByFlightNumberOfBooking([FromRoute]string flightNumber)
        {
            var passengersByFlightNumber = await _bookingsService.GetPassengersByFlightNumberOfBookingAsync(flightNumber);

            if (passengersByFlightNumber == null)
            {
                _logger.LogError($"Cannot find any passenger belong to the flight with transferred number '{flightNumber}'");
                return NotFound();
            }

            return Ok(passengersByFlightNumber);
        }
    }
}
