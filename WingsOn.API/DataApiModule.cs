using Autofac;
using WingsOn.Data.Logic;

namespace WingsOn.Api
{
    public class DataApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new DataLogicModule());
        }
    }
}
