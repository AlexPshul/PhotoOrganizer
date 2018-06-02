using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Business.Models;
using PhotoOrganizer.Infrastructure;
using PhotoOrganizer.Services.Interfaces;

namespace PhotoOrganizer.Business.Implementations
{
    internal class AlbumsManager : IAlbumsManager
    {
        private const string AlbumsGroup = "Albums";

        private readonly ILocalStorageService _localStorageService;
        private readonly IFileSystemService _fileSystemService;

        public AlbumsManager(ILocalStorageService localStorageService, IFileSystemService fileSystemService)
        {
            _localStorageService = localStorageService;
            _fileSystemService = fileSystemService;
        }

        public Task<bool> DoesAlbumSourceExists(string albumPath) => _fileSystemService.DoesDirectoryExists(albumPath);

        public Task<bool> IsValidDestinationForAlbum(string destinationPath) => _fileSystemService.IsValidDirectory(destinationPath);

        public Task<IReadOnlyCollection<string>> GetAvailableAlbums() => _localStorageService.GetAvailableKeys(AlbumsGroup);
        public async Task<bool> IsAlbumNameTaken(string albumName)
        {
            IReadOnlyCollection<string> availableAlbums = await GetAvailableAlbums();
            return availableAlbums.Contains(albumName ?? "");
        }

        public async Task<string> GetNextAvailableAlbumName(string baseName)
        {
            IReadOnlyCollection<string> availableAlbums = await GetAvailableAlbums();

            string availableName = baseName;
            bool isNameAvailable = availableAlbums.All(albumName => albumName != availableName);
            
            int serialNumber = 2;
            while (!isNameAvailable)
            {
                availableName = baseName + $" ({serialNumber++})";
                isNameAvailable = availableAlbums.All(albumName => albumName != availableName);
            }

            return availableName;
        }

        public async Task CreateAlbum(Album album)
        {
            string albumJson = await Task.Run(() => JsonConvert.SerializeObject(album));

            await _localStorageService.CreateString(AlbumsGroup, album.Name, albumJson);
        }

        public async Task<Album> GetAlbum(string name)
        {
            string albumJson = await _localStorageService.GetString(AlbumsGroup, name);

            return await Task.Run(() => JsonConvert.DeserializeObject<Album>(albumJson));
        }

        public Task DeleteAlbum(string name) => _localStorageService.DeleteString(AlbumsGroup, name);
    }
}