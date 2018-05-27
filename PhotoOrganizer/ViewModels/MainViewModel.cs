using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Controls;
using PhotoOrganizer.Infrastructure;
using PhotoOrganizer.Text;
using PhotoOrganizer.UIInfrastructure;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    public class MainViewModel : ReactiveObject, IMainViewModel, ISupportsActivation
    {
        #region Private Members

        private readonly IAlbumsManager _albumsManager;

        #endregion

        #region Properties

        private ObservableCollection<string> _availableAlbums;
        public ObservableCollection<string> AvailableAlbums
        {
            get => _availableAlbums;
            set => this.RaiseAndSetIfChanged(ref _availableAlbums, value);
        }

        public ReactiveCommand DeleteAlbumCommand { get; }

        private bool _fetchingAlbums;
        public bool FetchingAlbums
        {
            get => _fetchingAlbums;
            set => this.RaiseAndSetIfChanged(ref _fetchingAlbums, value);
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        #endregion

        #region Constructors

        public MainViewModel(IAlbumsManager albumsManager)
        {
            _albumsManager = albumsManager;

            this.WhenActivated(() => LoadAlbums());

            DeleteAlbumCommand = ReactiveCommand.CreateFromTask<string>(DeleteAlbum);
        }

        #endregion

        #region Private Methods

        private async Task DeleteAlbum(string albumName)
        {
            CustomContentDialog deleteDialog = new CustomContentDialog
            {
                Title = StringsReader.Get("Title_DeleteAlbum"),
                Content = string.Format(StringsReader.Get("Content_DeleteAlbumConfirmation"), albumName),
                PrimaryButtonText = StringsReader.Get("Button_DeletePrimary"),
                SecondaryButtonText = StringsReader.Get("Button_DeleteSecondary")
            };

            ContentDialogResult contentDialogResult = await deleteDialog.ShowAsync();
            if (contentDialogResult != ContentDialogResult.Primary)
                return;

            await _albumsManager.DeleteAlbum(albumName);

            AvailableAlbums.Remove(albumName);
        }

        private async void LoadAlbums()
        {
            FetchingAlbums = true;
            using (Disposable.Create(() => FetchingAlbums = false))
            {
                IReadOnlyCollection<string> albums = await _albumsManager.GetAvailableAlbums();
                AvailableAlbums = new ObservableCollection<string>(albums);
            }
        }

        #endregion
    }
}