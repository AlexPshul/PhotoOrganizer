using Autofac;
using PhotoOrganizer.Services.Interfaces;
using PhotoOrganizer.WindowsServices.Implementations;

namespace PhotoOrganizer.WindowsServices
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LocalStorageService>().As<ILocalStorageService>().SingleInstance();
        }
    }
}