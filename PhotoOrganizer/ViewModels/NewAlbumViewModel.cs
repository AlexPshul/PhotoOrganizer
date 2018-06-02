using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Business.Models;
using PhotoOrganizer.Text;
using PhotoOrganizer.UIInfrastructure;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    internal class NewAlbumViewModel : ReactiveObject, INewAlbumViewModel, ISupportsActivation
    {
        #region Consts

        private static readonly string NewAlbumBaseName = StringsReader.Get("Title_NewAlbumBaseName");

        #endregion

        #region Private Members

        private readonly IAlbumsManager _albumManager;

        private string _nextAvailableName = NewAlbumBaseName;

        #endregion

        #region Properties

        public ReactiveCommand<Unit, Album> CreateCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public ReactiveCommand FindSourceFolderCommand { get; }
        public ReactiveCommand InitialFindSourceFolderCommand { get; }
        public ReactiveCommand FindDestinationFoldercommand { get; }
        public ReactiveCommand InitialFindDestinationFolderCommand { get; }

        public ReactiveCommand UpdateNextAvailableAlbumNameCommand { get; }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _sourceFolder;
        public string SourceFolder
        {
            get => _sourceFolder;
            set => this.RaiseAndSetIfChanged(ref _sourceFolder, value);
        }


        private string _destinationFolder;
        public string DestinationFolder
        {
            get => _destinationFolder;
            set => this.RaiseAndSetIfChanged(ref _destinationFolder, value);
        }

        private readonly ObservableAsPropertyHelper<bool> _isSourceInvalidObservable;
        public bool IsSourceInvalid => _isSourceInvalidObservable?.Value ?? true;

        private readonly ObservableAsPropertyHelper<bool> _isDestinationInvalidObservable;
        public bool IsDestinationInvalid => _isDestinationInvalidObservable?.Value ?? true;

        private readonly ObservableAsPropertyHelper<bool> _isNameInUse;
        public bool IsNameInUse => _isNameInUse?.Value ?? false;

        private readonly ObservableAsPropertyHelper<bool> _isCreatingAlbum;
        public bool IsCreatingAlbum => _isCreatingAlbum?.Value ?? false;

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        #endregion

        #region Events

        public IObservable<Album> Created => CreateCommand.AsObservable();
        public IObservable<Unit> Canceled => CancelCommand.AsObservable();

        #endregion

        #region Constructors

        public NewAlbumViewModel(IAlbumsManager albumManager)
        {
            _albumManager = albumManager;

            IObservable<bool> canCreate = this.WhenAnyValue(
                self => self.Name,
                self => self.IsNameInUse,
                self => self.IsSourceInvalid,
                self => self.IsDestinationInvalid,
                IsNewAlbumCreationViable);
            
            this.WhenActivated(async () => await UpdateNextAvailableAlbumName());

            CreateCommand = ReactiveCommand.CreateFromTask(CreateAlbum, canCreate);
            CancelCommand = ReactiveCommand.Create(CancelAlbumCreation);

            _isCreatingAlbum = CreateCommand.IsExecuting.ToProperty(this, self => self.IsCreatingAlbum);

            _isSourceInvalidObservable = this.WhenAnyValue(self => self.SourceFolder)
                .Select(source => _albumManager.DoesAlbumSourceExists(source).ToObservable().Select(_ => !_))
                .Switch()
                .ObserveOnDispatcher()
                .ToProperty(this, self => self.IsSourceInvalid);

            _isDestinationInvalidObservable = this.WhenAnyValue(self => self.DestinationFolder)
                .Select(destination => _albumManager.IsValidDestinationForAlbum(destination).ToObservable().Select(_ => !_))
                .Switch()
                .ObserveOnDispatcher()
                .ToProperty(this, self => self.IsDestinationInvalid);

            _isNameInUse = this.WhenAnyValue(self => self.Name)
                .Where(name => name != null)
                .Select(name => name.Trim())
                .Select(name => _albumManager.IsAlbumNameTaken(name).ToObservable())
                .Switch()
                .ObserveOnDispatcher()
                .ToProperty(this, self => self.IsNameInUse);

            FindSourceFolderCommand = ReactiveCommand.Create(FindSourceFolder);
            IObservable<bool> canInitialSourceFind = this.WhenAnyValue(self => self.SourceFolder, string.IsNullOrEmpty);
            InitialFindSourceFolderCommand = ReactiveCommand.Create(FindSourceFolder, canInitialSourceFind);

            FindDestinationFoldercommand = ReactiveCommand.Create(FindDestinationFolder);
            IObservable<bool> canInitialDestinationFind = this.WhenAnyValue(self => self.DestinationFolder, string.IsNullOrEmpty);
            InitialFindDestinationFolderCommand = ReactiveCommand.Create(FindDestinationFolder, canInitialDestinationFind);

            UpdateNextAvailableAlbumNameCommand = ReactiveCommand.CreateFromTask(() => UpdateNextAvailableAlbumName(true));
        }

        #endregion

        #region Private Methods

        private async Task UpdateNextAvailableAlbumName(bool checkForEmpty = false)
        {
            if (!checkForEmpty || string.IsNullOrEmpty(Name))
            {
                _nextAvailableName = await _albumManager.GetNextAvailableAlbumName(NewAlbumBaseName);
                Name = _nextAvailableName;
            }
        }

        private bool IsNewAlbumCreationViable(string name, bool isNameInUse, bool isSourceInvalid, bool isDestinationInvalid)
        {
            return !string.IsNullOrEmpty(name) &&
                   !isNameInUse &&
                   !isSourceInvalid &&
                   !isDestinationInvalid;
        }

        private async Task<Album> CreateAlbum()
        {
            Album newAlbum = new Album { Name = Name, Source = SourceFolder, Destination = DestinationFolder };
            await _albumManager.CreateAlbum(newAlbum);
            
            Clear();

            return newAlbum;
        }

        private Unit CancelAlbumCreation()
        {
            Clear();
            return Unit.Default;
        }

        private async void Clear()
        {
            await UpdateNextAvailableAlbumName();
            SourceFolder = string.Empty;
            DestinationFolder = string.Empty;
        }

        private void FindSourceFolder()
        {
            PickFolder()
                .ToObservable()
                .ObserveOnDispatcher()
                .Subscribe(newSource => SourceFolder = newSource);
        }

        private void FindDestinationFolder()
        {
            PickFolder()
                .ToObservable()
                .ObserveOnDispatcher()
                .Subscribe(newDestination => DestinationFolder = newDestination);
        }

        private async Task<string> PickFolder()
        {
            FolderPicker folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                ViewMode = PickerViewMode.Thumbnail,
                FileTypeFilter = { "*" }
            };

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();
            return storageFolder?.Path ?? string.Empty;
        }

        #endregion
    }
}