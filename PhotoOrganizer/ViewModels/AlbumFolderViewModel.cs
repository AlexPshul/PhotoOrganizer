using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Business.Models;

namespace PhotoOrganizer.ViewModels
{
    internal class AlbumFolderViewModel : IAlbumFolderViewModel
    {
        public string RelativePath { get; }
        public IReadOnlyCollection<string> Images { get; }

        private AlbumFolderViewModel(ICurrentAlbumManager currentAlbumManager, AlbumFolder albumFolder)
        {
            RelativePath = albumFolder.FullPath.Replace(currentAlbumManager.CurrentAlbum.Source, "").TrimStart('\\', '/');
            Images = albumFolder.Images;
        }

        public class Factory : IAlbumFolderViewModelFactory
        {
            private readonly ICurrentAlbumManager _currentAlbumManager;

            public Factory(ICurrentAlbumManager currentAlbumManager)
            {
                _currentAlbumManager = currentAlbumManager;
            }

            public Task<IAlbumFolderViewModel> Create(AlbumFolder albumFolder)
            {
                IAlbumFolderViewModel albumFolderViewModel = new AlbumFolderViewModel(_currentAlbumManager, albumFolder);
                return Task.FromResult(albumFolderViewModel);
            }
        }
    }
}