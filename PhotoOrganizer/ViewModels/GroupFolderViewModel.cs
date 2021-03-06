﻿using System;
using System.IO;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Controls;
using PhotoOrganizer.Infrastructure;
using PhotoOrganizer.Text;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    public class GroupFolderViewModel : ReactiveObject, IGroupFolderViewModel, ISupportsActivation
    {
        #region Private Members

        private readonly ICurrentAlbumManager _currentAlbumManager;
        private readonly IShortcutsManager _shortcutsManager;

        #endregion

        #region Properties

        private string _groupPath;
        public string GroupPath
        {
            get => _groupPath;
            set => this.RaiseAndSetIfChanged(ref _groupPath, value);
        }

        private int _index;
        public int Index
        {
            get => _index;
            set => this.RaiseAndSetIfChanged(ref _index, value);
        }

        public ReactiveCommand<Unit, string> GroupLogicCommand { get; }
        public ReactiveCommand<Unit, bool> DeleteGroupCommand { get; }
        public ReactiveCommand<string, Unit> RenameCommand { get; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        private ObservableAsPropertyHelper<bool> _isInDestinationFolderHelper;
        public bool IsInDestinationFolder => _isInDestinationFolderHelper?.Value ?? false;

        private readonly ObservableAsPropertyHelper<bool> _isExecutingHelper;
        public bool IsExecuting => _isExecutingHelper?.Value ?? false;

        #endregion

        #region Events

        public IObservable<IGroupFolderViewModel> GroupDeleted => DeleteGroupCommand.Where(result => result).Select(_ => this);

        #endregion

        #region Constructors

        private GroupFolderViewModel(string title, ICurrentAlbumManager currentAlbumManager, IShortcutsManager shortcutsManager)
        {
            GroupPath = title;
            _currentAlbumManager = currentAlbumManager;
            _shortcutsManager = shortcutsManager;

            IObservable<bool> currentPhotoObservable = _currentAlbumManager.CurrentPhotoChanged
                .Select(currentPhoto => currentPhoto != null)
                .StartWith(_currentAlbumManager.CurrentPhoto != null);
            GroupLogicCommand = ReactiveCommand.CreateFromTask(ToggleCurrentPhoto, currentPhotoObservable);
            _isExecutingHelper = GroupLogicCommand.IsExecuting
                .Throttle(TimeSpan.FromMilliseconds(100))
                .ObserveOnDispatcher()
                .ToProperty(this, self => self.IsExecuting);

            DeleteGroupCommand = ReactiveCommand.CreateFromTask(_ => DeleteGroup());
            RenameCommand = ReactiveCommand.CreateFromTask<string>(RenameGroup);

            this.WhenActivated(disposables =>
            {
                _isInDestinationFolderHelper = _currentAlbumManager.CurrentPhotoChanged
                    .Select(_ => Unit.Default)
                    .Merge(GroupLogicCommand.Select(_ => Unit.Default))
                    .StartWith(Unit.Default)
                    .Select(_ => _currentAlbumManager.IsCurrentPhotoInFolder(GroupPath).ToObservable())
                    .Switch()
                    .ObserveOnDispatcher()
                    .ToProperty(this, self =>  self.IsInDestinationFolder);
                _isInDestinationFolderHelper.MergeToComposite(disposables);

                _shortcutsManager.GroupShortcutExecuted
                    .Where(pressedIndex => pressedIndex == Index)
                    .Where(_ => _currentAlbumManager.CurrentPhoto != null)
                    .Throttle(TimeSpan.FromMilliseconds(50))
                    .Select(_ => Unit.Default)
                    .ObserveOnDispatcher()
                    .InvokeCommand(GroupLogicCommand)
                    .MergeToComposite(disposables);
            });
        }

        public class Factory : IGroupFolderViewModelFactory
        {
            private readonly ICurrentAlbumManager _currentAlbumManager;
            private readonly IShortcutsManager _shortcutsManager;

            public Factory(ICurrentAlbumManager currentAlbumManager, IShortcutsManager shortcutsManager)
            {
                _currentAlbumManager = currentAlbumManager;
                _shortcutsManager = shortcutsManager;
            }

            public IGroupFolderViewModel Create(string title) => new GroupFolderViewModel(title, _currentAlbumManager, _shortcutsManager);
        }

        #endregion

        #region Private Methods

        private async Task<bool> DeleteGroup()
        {
            string confirmation = StringsReader.Get("Content_DeleteGroupConfirmation");
            string details = StringsReader.Get("Content_DeleteGroupDetails");
            CustomContentDialog deleteDialog = new CustomContentDialog(confirmation, Path.GetFileName(GroupPath), details)
            {
                Title = StringsReader.Get("Title_DeleteGroup"),
                PrimaryButtonText = StringsReader.Get("Button_DeletePrimary"),
                SecondaryButtonText = StringsReader.Get("Button_DeleteSecondary")
            };

            ContentDialogResult contentDialogResult = await deleteDialog.ShowAsync();
            if (contentDialogResult != ContentDialogResult.Primary)
                return false;

            await _currentAlbumManager.RemoveAlbumFolder(GroupPath);
            return true;
        }

        private async Task RenameGroup(string newName)
        {
            if (Path.GetFileName(GroupPath) == newName)
                return;

            GroupPath = await _currentAlbumManager.RenameAlbumFolder(GroupPath, newName);
        }

        private async Task<string> ToggleCurrentPhoto()
        {
            if (IsInDestinationFolder)
                await _currentAlbumManager.RemoveCurrentPhotoFromFolder(GroupPath);
            else
                await _currentAlbumManager.AddCurrentPhotoToFolder(GroupPath);

            return Path.Combine(GroupPath, Path.GetFileName(_currentAlbumManager.CurrentPhoto));
        }

        #endregion
    }
}