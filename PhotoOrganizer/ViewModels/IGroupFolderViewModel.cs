using System;
using System.Reactive;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    public interface IGroupFolderViewModel : IGroupItemViewModel
    {
        string GroupPath { get; }

        IObservable<IGroupFolderViewModel> GroupDeleted { get; }
    }
}