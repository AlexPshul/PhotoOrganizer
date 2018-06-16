using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Controls;
using PhotoOrganizer.Text;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    public class GroupFolderViewModel : ReactiveObject, IGroupFolderViewModel
    {
        #region Private Members

        private readonly ICurrentAlbumManager _currentAlbumManager;

        #endregion

        #region Properties

        private string _groupPath;
        public string GroupPath
        {
            get => _groupPath;
            set => this.RaiseAndSetIfChanged(ref _groupPath, value);
        }

        private int _index;
        public int Index
        {
            get => _index;
            set => this.RaiseAndSetIfChanged(ref _index, value);
        }

        public ReactiveCommand<Unit, string> ExecuteGroupLogicCommand { get; }
        public ReactiveCommand<Unit, bool> DeleteGroupCommand { get; }
        public ReactiveCommand<string, Unit> RenameCommand { get; }

        #endregion

        #region Events

        public IObservable<IGroupFolderViewModel> GroupDeleted => DeleteGroupCommand.Where(result => result).Select(_ => this);

        #endregion

        #region Constructors

        private GroupFolderViewModel(string title, ICurrentAlbumManager currentAlbumManager)
        {
            GroupPath = title;
            _currentAlbumManager = currentAlbumManager;
            ExecuteGroupLogicCommand = ReactiveCommand.CreateFromTask(AddCurrentPhotoToGroup);
            DeleteGroupCommand = ReactiveCommand.CreateFromTask(_ => DeleteGroup());
            RenameCommand = ReactiveCommand.CreateFromTask<string>(RenameGroup);
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
            if (await _currentAlbumManager.IsCurrentPhotoInFolder(GroupPath))
                return await _currentAlbumManager.RemoveCurrentPhotoFromFolder(GroupPath);
            
            return await _currentAlbumManager.AddCurrentPhotoToFolder(GroupPath);
        }

        private async Task<bool> DeleteGroup()
        {
            CustomContentDialog deleteDialog = new CustomContentDialog(StringsReader.Get("Content_DeleteGroupConfirmation"), Path.GetFileName(GroupPath))
            {
                Title = StringsReader.Get("Title_DeleteAlbum"),
                PrimaryButtonText = StringsReader.Get("Button_DeletePrimary"),
                SecondaryButtonText = StringsReader.Get("Button_DeleteSecondary")
            };

            ContentDialogResult contentDialogResult = await deleteDialog.ShowAsync();
            if (contentDialogResult != ContentDialogResult.Primary)
                return false;

            await _currentAlbumManager.RemoveAlbumFolder(GroupPath);
            return true;
        }

        private async Task RenameGroup(string newName)
        {
            await Task.CompletedTask;
        }

        #endregion
    }
}