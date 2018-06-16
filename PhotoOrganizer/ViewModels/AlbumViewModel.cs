using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Business.Models;
using PhotoOrganizer.Infrastructure;
using PhotoOrganizer.UIInfrastructure;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    internal class AlbumViewModel : ReactiveObject, IAlbumViewModel, ISupportsActivation
    {
        #region Private Members

        private readonly ICurrentAlbumManager _currentAlbumManager;
        private readonly IAlbumFolderViewModelFactory _albumFolderViewModelFactory;

        #endregion

        #region Properties

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public ReactiveCommand CloseAlbumCommand { get; }
        public ReactiveCommand<Unit, IReadOnlyCollection<IAlbumFolderViewModel>> FetchAlbumDataCommand { get; }
        
        private readonly ObservableAsPropertyHelper<bool> _isFetchingPhotosHelper;
        public bool IsFetchingPhotos => _isFetchingPhotosHelper?.Value ?? false;

        private readonly ObservableAsPropertyHelper<IReadOnlyCollection<IAlbumFolderViewModel>> _allPhotosHelper;
        public IReadOnlyCollection<IAlbumFolderViewModel> AllPhotos => _allPhotosHelper?.Value;

        public Album Album => _currentAlbumManager.CurrentAlbum;

        public IAlbumGroupsViewModel AlbumGroupsViewModel { get; }

        private string _selectedPhoto;
        public string SelectedPhoto
        {
            get => _selectedPhoto;
            set => this.RaiseAndSetIfChanged(ref _selectedPhoto, value);
        }
        
        #endregion

        #region Constructors

        public AlbumViewModel(ICurrentAlbumManager currentAlbumManager, IAlbumFolderViewModelFactory albumFolderViewModelFactory, IAlbumGroupsViewModel albumGroupsViewModel)
        {
            AlbumGroupsViewModel = albumGroupsViewModel;
            _currentAlbumManager = currentAlbumManager;
            _albumFolderViewModelFactory = albumFolderViewModelFactory;

            FetchAlbumDataCommand = ReactiveCommand.CreateFromTask(FetchAlbumFolders);
            _isFetchingPhotosHelper = FetchAlbumDataCommand.IsExecuting.ToProperty(this, self => self.IsFetchingPhotos);
            _allPhotosHelper = FetchAlbumDataCommand.Execute().ToProperty(this, self => self.AllPhotos);

            CloseAlbumCommand = ReactiveCommand.Create(() => _currentAlbumManager.CloseCurrentAlbum());
        }

        #endregion

        #region Private Methods

        private async Task<IReadOnlyCollection<IAlbumFolderViewModel>> FetchAlbumFolders()
        {
            IReadOnlyCollection<AlbumFolder> albumFolders = await _currentAlbumManager.GetAllAlbumFolders();
            return await albumFolders.Select(_albumFolderViewModelFactory.Create).AwaitAll();
        }

        #endregion
    }
}