﻿using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using PhotoOrganizer.Business.Models;

namespace PhotoOrganizer.Business.Interfaces
{
    public interface ICurrentAlbumManager
    {
        Album CurrentAlbum { get; }
        string CurrentPhoto { get; set; }

        IObservable<Unit> AlbumOpened { get; }
        IObservable<Unit> AlbumClosed { get; }

        IObservable<string> CurrentPhotoChanged { get; }

        void OpenAlbum(Album album);
        void CloseCurrentAlbum();

        Task LaunchAlbumDestinationFolder();
        Task UpdateAlbumSubFolders(IEnumerable<string> updatedSubFolders);

        Task<IReadOnlyCollection<AlbumFolder>> GetAllAlbumFolders();
        Task<string> AddAlbumFolder(string baseName);
        Task<string> RenameAlbumFolder(string originalPath, string newName);
        Task RemoveAlbumFolder(string path);

        Task<bool> AddCurrentPhotoToFolder(string folderName);
        Task<bool> RemoveCurrentPhotoFromFolder(string folderName);
        Task<bool> IsCurrentPhotoInFolder(string folderName);
    }
}