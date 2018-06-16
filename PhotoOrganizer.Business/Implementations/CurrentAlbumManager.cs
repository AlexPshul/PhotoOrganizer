﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Business.Models;
using PhotoOrganizer.Infrastructure;
using PhotoOrganizer.Services.DataObjects;
using PhotoOrganizer.Services.Interfaces;

namespace PhotoOrganizer.Business.Implementations
{
    internal class CurrentAlbumManager : ICurrentAlbumManager
    {
        #region Consts

        private static readonly string[] SupportedImageFormats = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".ico" };

        #endregion

        #region Private Members

        private readonly ISubject<Unit> _albumOpened = new Subject<Unit>();
        private readonly ISubject<Unit> _albumClosed = new Subject<Unit>();
        private readonly ISubject<string> _currentPhotoChangedSubject = new Subject<string>();
        private readonly IFileSystemService _fileSystemService;
        
        #endregion

        #region Properties

        private Album _currentAlbum;
        public Album CurrentAlbum
        {
            get => _currentAlbum;
            set
            {
                _currentAlbum = value; 
                if (_currentAlbum == null)
                    _albumClosed.OnNext(Unit.Default);
                else
                    _albumOpened.OnNext(Unit.Default);
            }
        }

        private string _currentPhoto;
        public string CurrentPhoto
        {
            get => _currentPhoto;
            set
            {
                _currentPhoto = value; 
                _currentPhotoChangedSubject.OnNext(value);
            }
        }

        #endregion

        #region Events

        public IObservable<Unit> AlbumOpened => _albumOpened.AsObservable();
        public IObservable<Unit> AlbumClosed => _albumClosed.AsObservable();

        public IObservable<string> CurrentPhotoChanged => _currentPhotoChangedSubject.AsObservable();

        #endregion

        #region Constructors

        public CurrentAlbumManager(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }

        #endregion

        #region Public Methods

        public void OpenAlbum(Album album) => CurrentAlbum = album;
        public void CloseCurrentAlbum() => CurrentAlbum = null;
        public Task LaunchAlbumDestinationFolder() => _fileSystemService.OpenDirectoryInExplorer(CurrentAlbum.Destination);

        public async Task<IReadOnlyCollection<AlbumFolder>> GetAllAlbumFolders()
        {
            IReadOnlyCollection<FolderData> albumDataFolders = await _fileSystemService.GetAllFoldersData(CurrentAlbum.Source, CurrentAlbum.SubFolders, SupportedImageFormats);
            return albumDataFolders.Select(dataFolder => new AlbumFolder(dataFolder.FullPath, dataFolder.Files)).ToReadOnlyCollection();
        }

        public Task<string> AddAlbumFolder()
        {
            return Task.FromResult("");
        }

        public Task RemoveAlbumFolder(string name)
        {
            return Task.CompletedTask;
        }

        public Task<string> AddCurrentPhotoToFolder(string folderName)
        {
            return Task.FromResult("");
        }

        public Task<string> RemoveCurrentPhotoFromFolder(string folderName)
        {
            return Task.FromResult("");
        }

        public Task<bool> IsCurrentPhotoInFolder(string folderName)
        {
            return Task.FromResult(true);
        }

        #endregion
    }
}