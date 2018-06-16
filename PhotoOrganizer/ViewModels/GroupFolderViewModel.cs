using System.Reactive;
using System.Threading.Tasks;
using PhotoOrganizer.Business.Interfaces;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    public class GroupFolderViewModel : ReactiveObject, IGroupFolderViewModel
    {
        #region Private Members

        private readonly ICurrentAlbumManager _currentAlbumManager;

        #endregion

        #region Properties

        private string _title;
        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        public ReactiveCommand<Unit, string> ExecuteGroupLogicCommand { get; }

        #endregion

        #region Constructors

        private GroupFolderViewModel(string title, ICurrentAlbumManager currentAlbumManager)
        {
            Title = title;
            _currentAlbumManager = currentAlbumManager;
            ExecuteGroupLogicCommand = ReactiveCommand.CreateFromTask(AddCurrentPhotoToGroup);

        }

        public class Factory : IGroupFolderViewModelFactory
        {
            private readonly ICurrentAlbumManager _currentAlbumManager;

            public Factory(ICurrentAlbumManager currentAlbumManager)
            {
                _currentAlbumManager = currentAlbumManager;
            }

            public IGroupFolderViewModel Create(string title) => new GroupFolderViewModel(title, _currentAlbumManager);
        }

        #endregion

        #region Private Methods

        private async Task<string> AddCurrentPhotoToGroup()
        {
            if (await _currentAlbumManager.IsCurrentPhotoInFolder(Title))
                return await _currentAlbumManager.RemoveCurrentPhotoFromFolder(Title);
            
            return await _currentAlbumManager.AddCurrentPhotoToFolder(Title);
        }

        #endregion
    }
}