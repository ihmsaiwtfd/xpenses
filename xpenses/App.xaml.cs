using Autofac;
using Core;
using DAL;
using System.Windows;

namespace xpenses
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public IContainer Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new DataModule());
            /*Container = */builder.Build();
        }
    }
}
