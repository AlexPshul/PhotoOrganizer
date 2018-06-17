using System.Reactive;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    public interface IGroupItemViewModel
    {
        ReactiveCommand<Unit, string> GroupLogicCommand { get; }
    }
}