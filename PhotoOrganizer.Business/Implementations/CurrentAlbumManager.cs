using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Business.Models;

namespace PhotoOrganizer.Business.Implementations
{
    internal class CurrentAlbumManager : ICurrentAlbumManager
    {
        private readonly ISubject<Unit> _albumOpened = new Subject<Unit>();
        private readonly ISubject<Unit> _albumClosed = new Subject<Unit>();

        public IObservable<Unit> AlbumOpened => _albumOpened.AsObservable();
        public IObservable<Unit> AlbumClosed => _albumClosed.AsObservable();

        private Album _currentAlbum;
        public Album CurrentAlbum
        {
            get => _currentAlbum;
            set
            {
                _currentAlbum = value; 
                if (_currentAlbum == null)
                    _albumClosed.OnNext(Unit.Default);
                else
                    _albumOpened.OnNext(Unit.Default);
            }
        }

        public void OpenAlbum(Album album) => CurrentAlbum = album;
        public void CloseCurrentAlbum() => CurrentAlbum = null;
    }
}