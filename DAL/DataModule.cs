using Autofac;
using Core;
using Core.Interfaces;

namespace DAL
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Xml.EntryRepository>().As<IRepository<Entry>>().SingleInstance();
            builder.RegisterType<Xml.CategoryRepository>().As<IRepository<Category>>().SingleInstance();
            builder.RegisterType<Xml.EntryCategoryRelationRepository>().As<IRepository<EntryCategoryRelation>>().SingleInstance();
            builder.RegisterType<Xml.CategoryRelationRepository>().As<IRepository<CategoryRelation>>().SingleInstance();
        }
    }
}
