using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
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

        Task LaunchAlbumDestinationFolder();

        Task<IReadOnlyCollection<AlbumFolder>> GetAllAlbumFolders();
    }
}