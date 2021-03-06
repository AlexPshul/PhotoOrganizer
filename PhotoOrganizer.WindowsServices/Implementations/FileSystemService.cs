﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.System;
using PhotoOrganizer.Infrastructure;
using PhotoOrganizer.Services.DataObjects;
using PhotoOrganizer.Services.Interfaces;

namespace PhotoOrganizer.WindowsServices.Implementations
{
    internal class FileSystemService : IFileSystemService
    {
        public async Task<bool> DoesDirectoryExists(string path)
        {
            if (path == null)
                return false;

            try
            {
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> DoesFileExists(string folderPath, string fileName)
        {
            if (!await DoesDirectoryExists(folderPath) || fileName == null)
                return false;

            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(folderPath);
            IStorageItem storageItem = await folder.TryGetItemAsync(fileName);

            return storageItem != null;
        }

        public async Task<bool> IsValidDirectory(string destinationPath)
        {
            if (destinationPath == null)
                return false;

            try
            {
                Uri uri = new Uri(destinationPath, UriKind.Absolute);
                DirectoryInfo destinationInfo = await Task.Run(() => new DirectoryInfo(uri.LocalPath));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CopyFile(string source, string destinationFolderPath)
        {
            if (!await DoesFileExists(Path.GetDirectoryName(source), Path.GetFileName(source)))
                return false;

            if (!await DoesDirectoryExists(destinationFolderPath))
                return false;

            StorageFile sourceFile = await StorageFile.GetFileFromPathAsync(source);
            StorageFolder destinationFolder = await StorageFolder.GetFolderFromPathAsync(destinationFolderPath);
            try
            {
                await sourceFile.CopyAsync(destinationFolder);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteFile(string filePath)
        {
            if (!await DoesFileExists(Path.GetDirectoryName(filePath), Path.GetFileName(filePath)))
                return false;

            try
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(filePath);
                await file.DeleteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        public async Task OpenDirectoryInExplorer(string fullPath)
        {
            StorageFolder folderToOpen = await StorageFolder.GetFolderFromPathAsync(fullPath);
            await Launcher.LaunchFolderAsync(folderToOpen);
        }

        public async Task<IReadOnlyCollection<FolderData>> GetAllFoldersData(string fullPath, IReadOnlyCollection<string> foldersToIgnore = null, params string[] formats)
        {
            try
            {
                if (!await DoesDirectoryExists(fullPath))
                    return Enumerable.Empty<FolderData>().ToReadOnlyCollection();

                StorageFolder requestedFolder = await StorageFolder.GetFolderFromPathAsync(fullPath);
                HashSet<string> formatsHashSet = new HashSet<string>(formats ?? Enumerable.Empty<string>());
                return await GetAllFoldersData(requestedFolder, foldersToIgnore ?? new ReadOnlyCollection<string>(new string[0]), formatsHashSet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new List<FolderData>();
        }

        public async Task<string> CreateNewFolder(string baseFolderPath, string name)
        {
            try
            {
                if (!await DoesDirectoryExists(baseFolderPath))
                    return "";

                StorageFolder baseFolder = await StorageFolder.GetFolderFromPathAsync(baseFolderPath);
                StorageFolder storageFolder = await baseFolder.CreateFolderAsync(name, CreationCollisionOption.GenerateUniqueName);

                return storageFolder.Path;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }

        public async Task<string> RenameFolder(string path, string newName)
        {
            try
            {
                if (!await DoesDirectoryExists(path))
                    return "";

                StorageFolder folderToRename = await StorageFolder.GetFolderFromPathAsync(path);
                await folderToRename.RenameAsync(newName, NameCollisionOption.GenerateUniqueName);

                return folderToRename.Path;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }

        public async Task DeleteFolder(string folderFullPath)
        {
            if (!await DoesDirectoryExists(folderFullPath))
                return;

            StorageFolder folderToDelete = await StorageFolder.GetFolderFromPathAsync(folderFullPath);
            await folderToDelete.DeleteAsync();
        }

        private async Task<IReadOnlyCollection<FolderData>> GetAllFoldersData(StorageFolder storageFolder, IReadOnlyCollection<string> foldersToIgnore, HashSet<string> formats)
        {
            Task<IReadOnlyList<StorageFile>> folderFilesTask = storageFolder.GetFilesAsync(CommonFileQuery.DefaultQuery).AsTask();

            IReadOnlyList<StorageFolder> subFolders = await storageFolder.GetFoldersAsync();
            IEnumerable<StorageFolder> relevantSubFolders = subFolders.Where(folder => !foldersToIgnore.Contains(folder.Path));

            Task<IReadOnlyCollection<FolderData>[]> subFoldersDataTasks = relevantSubFolders.Select(subFolder => GetAllFoldersData(subFolder, foldersToIgnore, formats)).AwaitAll();

            IReadOnlyList<StorageFile> folderFiles = await folderFilesTask;
            IEnumerable<string> filteredFiles = folderFiles.Where(file => !formats.Any() || formats.Contains(file.FileType.ToLower())).Select(file => file.Path);

            FolderData currentFolderData = new FolderData(storageFolder.Path, filteredFiles);

            IReadOnlyCollection<FolderData>[] subFoldersData = await subFoldersDataTasks;

            return currentFolderData.Concat(subFoldersData.Flatten()).ToReadOnlyCollection();
        }
    }
}