using System;

namespace PhotoOrganizer.Business.Interfaces
{
    public interface IShortcutsManager
    {
        IObservable<int> GroupShortcutExecuted { get; }
    }
}