using Autofac;
using PhotoOrganizer.Business.Implementations;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Business.Models;

namespace PhotoOrganizer.Business
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AlbumsManager>().As<IAlbumsManager>().SingleInstance();
            builder.RegisterType<CurrentAlbumManager>().As<ICurrentAlbumManager>().SingleInstance();
            builder.RegisterType<ShortcutsManager>().As<IShortcutsManager>().SingleInstance();
        }
    }
}