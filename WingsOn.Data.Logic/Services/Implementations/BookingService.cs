using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WingsOn.Domain;
using WingsOn.Dal;
using WingsOn.Data.Logic.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace WingsOn.Data.Logic.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private static int counter = 100;
        private int _id = -1;
        public int Id
        {
            get
            {
                if (_id < 0)
                    _id = counter++;
                return _id;
            }
            set { this.Id = counter++; }
        }
        private readonly IRepository<Booking> _bookingRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(
            IRepository<Booking> bookingRepository,
            ILogger<BookingService> logger
            )
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        public async Task<IList<Person>> GetPassengersByFlightNumberOfBookingAsync(string flightNumber)
        {
            var bookingsList = (await _bookingRepository.GetAllAsync())
                .Where(x => x.Flight.Number == flightNumber).ToList();
            var passengersByFlightNumber = bookingsList.SelectMany(b => b.Passengers)
                .ToList();
            if (passengersByFlightNumber.Count == 0)
            {
                _logger.LogError($"Booking Service cannot find any passenger with passed flight number '{flightNumber}'");
                return null;
            }

            return passengersByFlightNumber;
        }
    }
}
