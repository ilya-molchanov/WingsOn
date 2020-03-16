using Autofac;
using AutoMapper;
using WingsOn.Dal;
using WingsOn.Data.Logic.Services.Implementations;
using WingsOn.Data.Logic.Services.Interfaces;
using WingsOn.Domain;

namespace WingsOn.Data.Logic
{
    public class DataLogicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PersonRepository>().As<IRepository<Person>>();
            builder.RegisterType<BookingRepository>().As<IRepository<Booking>>();
            builder.RegisterType<FlightRepository>().As<IRepository<Flight>>();
            builder.RegisterType<PassengerService>().As<IPassengerService>();
            builder.RegisterType<BookingService>().As<IBookingService>();

            RegisterMappings(builder);
        }

        private void RegisterMappings(ContainerBuilder builder)
        {
            builder.Register(ctx =>
            {
                var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMissingTypeMaps = true;
                });

                return configuration.CreateMapper();
            }).As<IMapper>().SingleInstance().PreserveExistingDefaults();
        }
    }
}
