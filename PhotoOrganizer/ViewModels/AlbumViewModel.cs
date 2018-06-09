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

        private string _albumName;
        public string AlbumName
        {
            get => _albumName;
            set => this.RaiseAndSetIfChanged(ref _albumName, value);
        }

        private ObservableAsPropertyHelper<string> _numberOfPhotosHelper;
        public string NumberOfPhotos => _numberOfPhotosHelper?.Value ?? string.Empty;
        
        public ReactiveCommand CloseAlbumCommand { get; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public ReactiveCommand<Unit, IReadOnlyCollection<AlbumFolder>> FetchAlbumData { get; }

        private readonly ObservableAsPropertyHelper<bool> _isFetchingPhotosHelper;
        public bool IsFetchingPhotos => _isFetchingPhotosHelper?.Value ?? false;

        #endregion

        #region Constructors

        public AlbumViewModel(ICurrentAlbumManager currentAlbumManager)
        {
            _currentAlbumManager = currentAlbumManager;
            FetchAlbumData = ReactiveCommand.CreateFromTask(_currentAlbumManager.GetAllAlbumFolders);
            _isFetchingPhotosHelper = FetchAlbumData.IsExecuting.ToProperty(this, self => self.IsFetchingPhotos);

            this.WhenActivated(() =>
            {
                AlbumName = _currentAlbumManager.CurrentAlbum.Name;
                _numberOfPhotosHelper = FetchAlbumData.Execute()
                    .Select(albumData => albumData
                        .Sum(folder => folder.Images.Count))
                    .Select(numberOfPhotos => $"Total photos: {numberOfPhotos}")
                    .ToProperty(this, self => self.NumberOfPhotos);
            });

            CloseAlbumCommand = ReactiveCommand.Create(() => _currentAlbumManager.CloseCurrentAlbum());
        }

        #endregion
    }
}