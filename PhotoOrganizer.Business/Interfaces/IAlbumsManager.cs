using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoOrganizer.Business.Models;

namespace PhotoOrganizer.Business.Interfaces
{
    public interface IAlbumsManager
    {
        Task<IReadOnlyCollection<string>> GetAvailableAlbums();
        Task CreateAlbum(Album album);
        Task<Album> GetAlbum(string name);
        Task DeleteAlbum(string name);
    }
}