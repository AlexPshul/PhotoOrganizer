using System.Reactive;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    public interface IAlbumGroupsViewModel
    {
        ReactiveCommand<int, Unit> CopyToGroupCommand { get; }
    }
}