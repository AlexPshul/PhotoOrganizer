using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Business.Models;
using PhotoOrganizer.UIInfrastructure;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    internal class AlbumViewModel : ReactiveObject, IAlbumViewModel, ISupportsActivation
    {
        #region Private Members

        private readonly ICurrentAlbumManager _currentAlbumManager;

        #endregion

        #region Properties

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public ReactiveCommand CloseAlbumCommand { get; }
        public ReactiveCommand<Unit, IReadOnlyCollection<AlbumFolder>> FetchAlbumDataCommand { get; }
        
        private readonly ObservableAsPropertyHelper<bool> _isFetchingPhotosHelper;
        public bool IsFetchingPhotos => _isFetchingPhotosHelper?.Value ?? false;

        private readonly ObservableAsPropertyHelper<IReadOnlyCollection<string>> _allPhotosHelper;
        public IReadOnlyCollection<string> AllPhotos => _allPhotosHelper?.Value;

        #endregion

        #region Constructors

        public AlbumViewModel(ICurrentAlbumManager currentAlbumManager)
        {
            _currentAlbumManager = currentAlbumManager;
            FetchAlbumDataCommand = ReactiveCommand.CreateFromTask(_currentAlbumManager.GetAllAlbumFolders);
            _isFetchingPhotosHelper = FetchAlbumDataCommand.IsExecuting.ToProperty(this, self => self.IsFetchingPhotos);
            _allPhotosHelper = FetchAlbumDataCommand.Execute()
                .SelectMany(albumFolders => albumFolders
                    .Select(folder => folder.Images))
                .ToProperty(this, self => self.AllPhotos);

            CloseAlbumCommand = ReactiveCommand.Create(() => _currentAlbumManager.CloseCurrentAlbum());
        }

        #endregion
    }
}