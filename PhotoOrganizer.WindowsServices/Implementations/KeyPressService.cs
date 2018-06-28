using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using PhotoOrganizer.Services.Interfaces;

namespace PhotoOrganizer.WindowsServices.Implementations
{
    internal class KeyPressService : IKeyPressService
    {
        private readonly ISubject<VirtualKey> _keyPressedSubject = new Subject<VirtualKey>();

        public IObservable<int> NumberKeyPressed { get; }

        public KeyPressService()
        {
            Window.Current.CoreWindow.KeyDown += (sender, args) => _keyPressedSubject.OnNext(args.VirtualKey);
            NumberKeyPressed = _keyPressedSubject
                .Where(_ => !(FocusManager.GetFocusedElement() is TextBox))
                .Select(key => key.ToString())
                .Select(keyString => keyString.Replace("Number", "").Replace("Pad", ""))
                .Where(potentialNumber => int.TryParse(potentialNumber, out int _))
                .Select(int.Parse)
                .Where(number => number > 0 && number < 10);
        }
    }
}