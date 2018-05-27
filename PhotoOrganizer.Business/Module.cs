﻿using Autofac;
using PhotoOrganizer.Business.Implementations;
using PhotoOrganizer.Business.Interfaces;

namespace PhotoOrganizer.Business
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AlbumsManager>().As<IAlbumsManager>().SingleInstance();
        }
    }
}