using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Controls;
using PhotoOrganizer.Infrastructure;
using PhotoOrganizer.Text;
using PhotoOrganizer.UIInfrastructure;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    internal class MainViewModel : ReactiveObject, IMainViewModel, ISupportsActivation
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

        public INewAlbumViewModel NewAlbumViewModel { get; }

        public ReactiveCommand CreateNewAlbumCommand { get; }
        public ReactiveCommand<string, Unit> DeleteAlbumCommand { get; }

        public ReactiveCommand LoadAlbumsCommand { get; }

        private readonly ObservableAsPropertyHelper<bool> _isFetchingAlbumns;
        public bool IsFetchingAlbums => _isFetchingAlbumns?.Value ?? false;

        private bool _isCreatingAlbum;
        public bool IsCreatingAlbum
        {
            get => _isCreatingAlbum;
            set => this.RaiseAndSetIfChanged(ref _isCreatingAlbum, value);
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
        
        #endregion

        #region Constructors

        public MainViewModel(INewAlbumViewModel newAlbumViewModel, IAlbumsManager albumsManager)
        {
            NewAlbumViewModel = newAlbumViewModel;
            NewAlbumViewModel.Canceled
                    .Merge(NewAlbumViewModel.Created.Select(_ => Unit.Default))
                    .Subscribe(_ => IsCreatingAlbum = false);

            _albumsManager = albumsManager;

            this.WhenActivated(async () => await LoadAlbums());

            CreateNewAlbumCommand = ReactiveCommand.Create(() => IsCreatingAlbum = true);
            DeleteAlbumCommand = ReactiveCommand.CreateFromTask<string>(DeleteAlbum);
            LoadAlbumsCommand = ReactiveCommand.CreateFromTask(LoadAlbums);

            DeleteAlbumCommand.InvokeCommand(NewAlbumViewModel.UpdateNextAvailableAlbumNameCommand);

            _isFetchingAlbumns = LoadAlbumsCommand.IsExecuting.ToProperty(this, self => self.IsFetchingAlbums);

            NewAlbumViewModel.Created.Select(_ => Unit.Default).InvokeCommand(LoadAlbumsCommand);
        }

        #endregion

        #region Private Methods

        private async Task DeleteAlbum(string albumName)
        {
            CustomContentDialog deleteDialog = new CustomContentDialog(StringsReader.Get("Content_DeleteAlbumConfirmation"), albumName)
            {
                Title = StringsReader.Get("Title_DeleteAlbum"),
                PrimaryButtonText = StringsReader.Get("Button_DeletePrimary"),
                SecondaryButtonText = StringsReader.Get("Button_DeleteSecondary")
            };

            ContentDialogResult contentDialogResult = await deleteDialog.ShowAsync();
            if (contentDialogResult != ContentDialogResult.Primary)
                return;

            await _albumsManager.DeleteAlbum(albumName);

            AvailableAlbums.Remove(albumName);
        }

        private async Task LoadAlbums()
        {
            IReadOnlyCollection<string> albums = await _albumsManager.GetAvailableAlbums();
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                AvailableAlbums = new ObservableCollection<string>(albums);
            });
        }

        #endregion
    }
}