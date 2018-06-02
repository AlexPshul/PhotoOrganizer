using System;
using System.Reactive;
using PhotoOrganizer.Business.Models;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    public interface INewAlbumViewModel
    {
        IObservable<Album> Created { get; }
        IObservable<Unit> Canceled { get; }

        ReactiveCommand UpdateNextAvailableAlbumNameCommand { get; }
    }
}