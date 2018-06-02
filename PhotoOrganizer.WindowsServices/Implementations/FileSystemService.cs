using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using PhotoOrganizer.Services.Interfaces;

namespace PhotoOrganizer.WindowsServices.Implementations
{
    internal class FileSystemService : IFileSystemService
    {
        public async Task<bool> DoesDirectoryExists(string path)
        {
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

        public async Task<bool> IsValidDirectory(string destinationPath)
        {
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
    }
}