using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Business.Models;
using PhotoOrganizer.Infrastructure;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    internal class AlbumGroupsViewModel : IAlbumGroupsViewModel
    {
        #region Private Members

        private readonly ICurrentAlbumManager _currentAlbumManager;
        private readonly SerialDisposable _deleteFolderGroupSubscriptions = new SerialDisposable();

        #endregion

        #region Properties

        public ObservableCollection<IGroupItemViewModel> Groups { get; }
        public Album Album => _currentAlbumManager.CurrentAlbum;

        public ReactiveCommand<Unit, Unit> OpenDestinationFolderCommand { get; }

        #endregion

        #region Constructors

        public AlbumGroupsViewModel(ICurrentAlbumManager currentAlbumManager, IGroupFolderViewModelFactory groupFolderFactory, IGroupCreatorViewModel groupCreator)
        {
            _currentAlbumManager = currentAlbumManager;

            IEnumerable<IGroupItemViewModel> groups = _currentAlbumManager.CurrentAlbum.SubFolders.Select(groupFolderFactory.Create);
            Groups = new ObservableCollection<IGroupItemViewModel>(groups.Concat(groupCreator));

            Observable
                .FromEventPattern<NotifyCollectionChangedEventArgs>(Groups, nameof(Groups.CollectionChanged))
                .Do(pattern => ReorderIfMovedToLast(pattern.EventArgs))
                .Do(_ => ResetIndexes())
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => UpdateCurrentAlbum());

            groupCreator.ExecuteGroupLogicCommand
                .Select(groupFolderFactory.Create)
                .Do(newGroup => Groups.Insert(Groups.Count - 1, newGroup))
                .Subscribe(_ => SubscribeToGroupDeletion());

            ResetIndexes();
            SubscribeToGroupDeletion();

            OpenDestinationFolderCommand = ReactiveCommand.CreateFromTask(_currentAlbumManager.LaunchAlbumDestinationFolder);
        }

        #endregion

        #region Private Methods

        private void UpdateCurrentAlbum()
        {
            _currentAlbumManager.UpdateAlbumSubFolders(Groups.OfType<IGroupFolderViewModel>().Select(folderGroup => folderGroup.GroupPath));
        }

        private void ResetIndexes()
        {
            Groups.OfType<IGroupFolderViewModel>().ForEach((item, index) => item.Index = index + 1);
        }

        private async void ReorderIfMovedToLast(NotifyCollectionChangedEventArgs args)
        {
            if (args.NewStartingIndex == Groups.Count - 1 && !(Groups[Groups.Count - 1] is IGroupCreatorViewModel))
            {
                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, SwitchLastItems);

                void SwitchLastItems()
                {
                    IGroupItemViewModel lastElement = Groups[Groups.Count - 1];
                    Groups[Groups.Count - 1] = Groups[Groups.Count - 2];
                    Groups[Groups.Count - 2] = lastElement;
                }
            }
        }

        private void SubscribeToGroupDeletion()
        {
            _deleteFolderGroupSubscriptions.Disposable = Groups
                .OfType<IGroupFolderViewModel>()
                .Select(folderGroup => folderGroup.GroupDeleted)
                .Merge()
                .Do(deletedFolder => Groups.Remove(deletedFolder))
                .Subscribe(_ => SubscribeToGroupDeletion());
        }

        #endregion
    }
}