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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new DataModule());
            IocProvider.Container = builder.Build();
        }
    }
}
