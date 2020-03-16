using System.Collections.Generic;
using System.Threading.Tasks;
using WingsOn.Domain;

namespace WingsOn.Data.Logic.Services.Interfaces
{
    public interface IPassengerService
    {
        Task<Person> GetPersonEntityAsync(int personId);

        Task<IList<Person>> GetPeopleByGenderAsync(GenderType genderType);

        Task<bool?> CreatePersonWithBookingAsync(Person person, int customerId, string flightNumber);

        Task<Person> UpdatePersonAddressAsync(int personId, string address);
    }
}
