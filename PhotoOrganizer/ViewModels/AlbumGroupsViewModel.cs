using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
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

            Groups.CollectionChanged += async (sender, args) =>
            {
                if (args.NewStartingIndex == Groups.Count - 1 && Groups[Groups.Count - 1] is IGroupCreatorViewModel)
                {
                    await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, SwitchLastItems);

                    void SwitchLastItems()
                    {
                        IGroupItemViewModel lastElement = Groups[Groups.Count - 1];
                        Groups[Groups.Count - 1] = Groups[Groups.Count - 2];
                        Groups[Groups.Count - 2] = lastElement;
                    }
                }
            };

            OpenDestinationFolderCommand = ReactiveCommand.CreateFromTask(_currentAlbumManager.LaunchAlbumDestinationFolder);
        }

        #endregion
    }
}