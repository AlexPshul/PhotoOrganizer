using System.Reactive;
using PhotoOrganizer.Business.Interfaces;
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
        
        public ReactiveCommand CloseAlbumCommand { get; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        #endregion

        #region Constructors

        public AlbumViewModel(ICurrentAlbumManager currentAlbumManager)
        {
            _currentAlbumManager = currentAlbumManager;

            this.WhenActivated(() => AlbumName = _currentAlbumManager.CurrentAlbum.Name);

            CloseAlbumCommand = ReactiveCommand.Create(() => _currentAlbumManager.CloseCurrentAlbum());
        }

        #endregion
    }
}