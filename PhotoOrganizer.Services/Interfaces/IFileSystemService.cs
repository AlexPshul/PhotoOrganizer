﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoOrganizer.Services.DataObjects;

namespace PhotoOrganizer.Services.Interfaces
{
    public interface IFileSystemService
    {
        Task<bool> DoesDirectoryExists(string path);
        Task<bool> DoesFileExists(string folderPath, string fileName);
        Task<bool> IsValidDirectory(string destinationPath);

        Task<bool> CopyFile(string source, string destinationFolder);
        Task<bool> DeleteFile(string filePath);

        Task OpenDirectoryInExplorer(string fullPath);

        Task<IReadOnlyCollection<FolderData>> GetAllFoldersData(string fullPath, IReadOnlyCollection<string> foldersToIgnore = null, params string[] formats);
        Task<string> CreateNewFolder(string baseFolderPath, string name);
        Task<string> RenameFolder(string path, string newName);
        Task DeleteFolder(string folderFullPath);
    }
}