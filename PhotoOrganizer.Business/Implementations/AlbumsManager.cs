using System.Collections.Generic;
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

        public AlbumsManager(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<IReadOnlyCollection<string>> GetAvailableAlbums()
        {
            await Task.Delay(5000);
            return new[] { "ALBUM 1", "ALBUM 2", "ALBUM 3" }.ToReadOnlyCollection();
            //return _localStorageService.GetAvailableKeys(AlbumsGroup);
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