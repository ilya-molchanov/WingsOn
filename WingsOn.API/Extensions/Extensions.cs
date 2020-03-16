using WingsOn.API.Web.Models;
using WingsOn.Domain;

namespace WingsOn.API.Extensions
{
    public static class Extensions
    {
        public static Person ToPersonEntity(this CreatePassengerRequest createPassengerRequest)
        {
            Person person = new Person();
            person.Name = createPassengerRequest.Name;
            person.DateBirth = createPassengerRequest.DateBirth;
            person.Gender = createPassengerRequest.Gender;
            person.Address = createPassengerRequest.Address;
            person.Email = createPassengerRequest.Email;
            return person;
        }
    }
}
