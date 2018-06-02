using System;
using System.Reactive;
using PhotoOrganizer.Business.Models;

namespace PhotoOrganizer.Business.Interfaces
{
    public interface ICurrentAlbumManager
    {
        IObservable<Unit> AlbumOpened { get; }
        IObservable<Unit> AlbumClosed { get; }

        Album CurrentAlbum { get; }

        void OpenAlbum(Album album);
        void CloseCurrentAlbum();
    }
}