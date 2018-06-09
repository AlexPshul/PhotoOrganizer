using System.Threading.Tasks;
using PhotoOrganizer.Business.Models;

namespace PhotoOrganizer.ViewModels
{
    public interface IAlbumFolderViewModelFactory
    {
        Task<IAlbumFolderViewModel> Create(AlbumFolder albumFolder);
    }
}