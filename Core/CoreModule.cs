using Autofac;
using Core.Interfaces.UseCases;
using Core.UseCases;

namespace Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AddEntryUseCase>().As<IAddEntryUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<GetEntriesUseCase>().As<IGetEntriesUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<DeleteEntriesUseCase>().As<IDeleteEntriesUseCase>().InstancePerLifetimeScope();
        }
    }
}
