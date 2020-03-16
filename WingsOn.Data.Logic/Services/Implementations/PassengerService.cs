using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WingsOn.Domain;
using WingsOn.Dal;
using WingsOn.Data.Logic.Services.Interfaces;
using WingsOn.Data.Logic.Exceptions;
using Microsoft.Extensions.Logging;

namespace WingsOn.Data.Logic.Services.Implementations
{
    public class PassengerService : IPassengerService
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
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<Flight> _flightRepository;
        private readonly ILogger<PassengerService> _logger;

        public PassengerService(
            IRepository<Person> personRepository,
            IRepository<Booking> bookingRepository,
            IRepository<Flight> flightRepository,
            ILogger<PassengerService> logger
            )
        {
            _personRepository = personRepository;
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
            _logger = logger;
        }

        public async Task<Person> GetPersonEntityAsync(int personId)
        {
            var person = await _personRepository.GetAsync(personId);
            if (person == null)
            {
                return null;
            }

            return person;
        }
        
        public async Task<IList<Person>> GetPeopleByGenderAsync(GenderType genderType)
        {
            var passengersByGender = (await _personRepository.GetAllAsync())
                .Where(x => x.Gender == genderType).ToList();
            if (passengersByGender.Count == 0)
            {
                _logger.LogError($"Passenger Service cannot find any passenger with passed gender Type '{genderType}'");
                return null;
            }

            return passengersByGender;
        }

        public async Task<bool?> CreatePersonWithBookingAsync(Person person, int customerId, string flightNumber)
        {
            person.Id = Id;
            int newPersonId;
            try
            {
                newPersonId = (await _personRepository.SaveAsync(person)).Id;
            }
            catch (Exception e)
            {
                _logger.LogError($"Passenger Service cannot save a passenger with provided data");
                return null;
            }

            var currentFlight = (await _flightRepository.GetAllAsync())
                .FirstOrDefault(x => x.Number == flightNumber);

            var customer = (await _personRepository.GetAsync(customerId));

            if (currentFlight != null && customer != null)
            {
                try
                {
                    Booking newlyCreatedBooking = new Booking();
                    newlyCreatedBooking.Number = "WO-" + new Random().Next(100000, 999999).ToString();
                    newlyCreatedBooking.Flight = currentFlight;
                    newlyCreatedBooking.Customer = customer;
                    newlyCreatedBooking.DateBooking = DateTime.Now;
                    newlyCreatedBooking.Passengers = new[]
                    {
                        await _personRepository.GetAsync(newPersonId)
                    };
                    var result = (await _bookingRepository.SaveAsync(newlyCreatedBooking));
                    return result != null;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return false;
                }
            }
            else
            {
                _logger.LogError($"Passenger Service cannot find customer or flight because of invalid  data. Please, check passed data");
            }

            return false;
        }

        public async Task<Person> UpdatePersonAddressAsync(int personId, string address)
        {
            try
            {
                var person = await _personRepository.GetAsync(personId);

                if (person == null)
                {
                    throw new PersonNotExistsException($"Person '{personId}' does not exist");
                }

                person.Address = address;

                return await _personRepository.SaveAsync(person);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }
    }
}
