using Autofac;

namespace Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
           // builder.RegisterType<Xml.EntryRepository>().As<IRepository<Entry>>().SingleInstance();
        }
    }
}
