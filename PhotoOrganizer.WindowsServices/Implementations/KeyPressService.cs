using System;
using System.Reactive.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using PhotoOrganizer.Services.Interfaces;

namespace PhotoOrganizer.WindowsServices.Implementations
{
    internal class KeyPressService : IKeyPressService
    {
        public IObservable<int> NumberKeyPressed { get; }

        public KeyPressService()
        {
            NumberKeyPressed = Observable
                .FromEventPattern<KeyEventArgs>(CoreWindow.GetForCurrentThread(), nameof(CoreWindow.KeyDown))
                .Where(_ => !(FocusManager.GetFocusedElement() is TextBox))
                .Select(pattern => pattern.EventArgs.VirtualKey.ToString())
                .Select(keyString => keyString.Replace("Number", "").Replace("Pad", ""))
                .Where(potentialNumber => int.TryParse(potentialNumber, out int _))
                .Select(int.Parse)
                .Where(number => number > 0 && number < 10);
        }
    }
}