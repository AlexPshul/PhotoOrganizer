using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using PhotoOrganizer.Infrastructure;
using PhotoOrganizer.Services.Interfaces;

namespace PhotoOrganizer.WindowsServices.Implementations
{
    internal class LocalStorageService : ILocalStorageService
    {
        public async Task<string> GetString(string group, string key)
        {
            StorageFile concreteFile = await GetFileByName(group, key);

            return await FileIO.ReadTextAsync(concreteFile);
        }

        public async Task<IReadOnlyCollection<string>> GetAvailableKeys(string group)
        {
            StorageFolder groupFolder = await GetFolderByName(group);
            IReadOnlyList<StorageFile> availableFiles = await groupFolder.GetFilesAsync();

            return availableFiles.Select(file => file.Name).ToReadOnlyCollection();
        }

        public async Task CreateString(string group, string key, string value)
        {
            StorageFile concreteFile = await GetFileByName(group, key);

            await FileIO.WriteTextAsync(concreteFile, value);
        }

        public async Task DeleteString(string group, string key)
        {
            StorageFile concreteFile = await GetFileByName(group, key);

            await concreteFile.DeleteAsync();
        }

        private static async Task<StorageFolder> GetFolderByName(string group)
        {
            StorageFolder currentSharedLocalFolder = ApplicationData.Current.LocalFolder;
            IStorageItem groupItem = await currentSharedLocalFolder.TryGetItemAsync(group);

            return groupItem?.IsOfType(StorageItemTypes.Folder) != true
                ? await currentSharedLocalFolder.CreateFolderAsync(group, CreationCollisionOption.ReplaceExisting)
                : (StorageFolder)groupItem;
        }

        private static async Task<StorageFile> GetFileByName(string group, string key)
        {
            StorageFolder groupFolder = await GetFolderByName(group);

            IStorageItem fileItem = await groupFolder.TryGetItemAsync(key);
            return fileItem?.IsOfType(StorageItemTypes.File) != true
                ? await groupFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting)
                : (StorageFile)fileItem;
        }
    }
}