using System;

namespace PhotoOrganizer.Services.Interfaces
{
    public interface IKeyPressService
    {
        IObservable<int> NumberKeyPressed { get; }
    }
}