using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoOrganizer.Services.DataObjects;

namespace PhotoOrganizer.Services.Interfaces
{
    public interface IFileSystemService
    {
        Task<bool> DoesDirectoryExists(string path);
        Task<bool> IsValidDirectory(string destinationPath);

        Task OpenDirectoryInExplorer(string fullPath);

        Task<IReadOnlyCollection<FolderData>> GetAllFoldersData(string fullPath, IReadOnlyCollection<string> foldersToIgnore = null, params string[] formats);
    }
}