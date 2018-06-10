using System.Collections.ObjectModel;
using System.Reactive;
using Windows.System;
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

        public ObservableCollection<string> Groups { get; }
        public Album Album => _currentAlbumManager.CurrentAlbum;

        public ReactiveCommand<Unit, Unit> OpenDestinationFolderCommand { get; }

        #endregion

        #region Constructors

        public AlbumGroupsViewModel(ICurrentAlbumManager currentAlbumManager)
        {
            _currentAlbumManager = currentAlbumManager;
            Groups = new ObservableCollection<string>(_currentAlbumManager.CurrentAlbum.SubFolders.Concat("Group 1").Concat("Group 2").Concat(""));

            OpenDestinationFolderCommand = ReactiveCommand.CreateFromTask(_currentAlbumManager.LaunchAlbumDestinationFolder);
        }

        #endregion
    }
}