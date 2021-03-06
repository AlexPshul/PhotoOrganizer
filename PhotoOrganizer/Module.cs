﻿using System.Linq;
using Autofac;
using PhotoOrganizer.Infrastructure;
using PhotoOrganizer.UIInfrastructure;
using PhotoOrganizer.ViewModels;
using ReactiveUI;

namespace PhotoOrganizer
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterViewModels(builder);
            builder.RegisterType<CustomViewLocator>().As<IViewLocator>().SingleInstance();
        }

        private void RegisterViewModels(ContainerBuilder builder)
        {
            ThisAssembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .Where(type => type.GetInterfaces()
                    .Any(interfaceType => interfaceType.Name.StartsWith("I") && interfaceType.Name.EndsWith("ViewModel")))
                .ForEach(type => builder.RegisterType(type).AsImplementedInterfaces());

            builder.RegisterType<AlbumFolderViewModel.Factory>().As<IAlbumFolderViewModelFactory>().SingleInstance();
            builder.RegisterType<GroupFolderViewModel.Factory>().As<IGroupFolderViewModelFactory>().SingleInstance();
        }
    }
}