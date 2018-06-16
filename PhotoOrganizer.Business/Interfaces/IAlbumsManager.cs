using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoOrganizer.Business.Models;

namespace PhotoOrganizer.Business.Interfaces
{
    public interface IAlbumsManager
    {
        Task<bool> DoesAlbumSourceExists(string albumPath);
        Task<bool> IsValidDestinationForAlbum(string destinationPath);

        Task<IReadOnlyCollection<Album>> GetAvailableAlbums();
        Task<bool> IsAlbumNameTaken(string albumName);
        Task<string> GetNextAvailableAlbumName(string baseName);
        Task CreateAlbum(Album album);
        Task UpdateAlbum(Album album);
        Task<Album> GetAlbum(string name);
        Task DeleteAlbum(string name);
    }
}