using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WingsOn.Domain;

namespace WingsOn.Data.Logic.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IList<Person>> GetPassengersByFlightNumberOfBookingAsync(string flightNumber);
    }
}
