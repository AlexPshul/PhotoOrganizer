using System;
using System.Reactive.Linq;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Services.Interfaces;

namespace PhotoOrganizer.Business.Models
{
    internal class ShortcutsManager : IShortcutsManager
    {
        public IObservable<int> GroupShortcutExecuted { get; }

        public ShortcutsManager(IKeyPressService keyPressService)
        {
            GroupShortcutExecuted = keyPressService.NumberKeyPressed.Publish().RefCount();
        }
    }
}