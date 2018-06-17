using System.Reactive;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Text;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    internal class GroupCreatorViewModel : IGroupCreatorViewModel
    {
        private static readonly string NewGroupBaseName = StringsReader.Get("Title_NewGroupBaseName");

        public ReactiveCommand<Unit, string> GroupLogicCommand { get; }

        public GroupCreatorViewModel(ICurrentAlbumManager currentAlbumManager)
        {
            GroupLogicCommand = ReactiveCommand.CreateFromTask(() => currentAlbumManager.AddAlbumFolder(NewGroupBaseName));
        }
    }
}