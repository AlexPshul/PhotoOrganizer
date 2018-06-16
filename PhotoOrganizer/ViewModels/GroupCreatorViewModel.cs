using System.Reactive;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Text;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    internal class GroupCreatorViewModel : IGroupCreatorViewModel
    {
        private static readonly string NewGroupBaseName = StringsReader.Get("Title_NewGroupBaseName");

        public ReactiveCommand<Unit, string> ExecuteGroupLogicCommand { get; }

        public GroupCreatorViewModel(ICurrentAlbumManager currentAlbumManager)
        {
            ExecuteGroupLogicCommand = ReactiveCommand.CreateFromTask(() => currentAlbumManager.AddAlbumFolder(NewGroupBaseName));
        }
    }
}