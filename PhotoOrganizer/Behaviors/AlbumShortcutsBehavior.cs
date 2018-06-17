using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;

namespace PhotoOrganizer.Behaviors
{
    public class AlbumShortcutsBehavior : Behavior<FrameworkElement>
    {
        private static readonly HashSet<VirtualKey> NavigationKeys = new HashSet<VirtualKey>
        {
            VirtualKey.Down,
            VirtualKey.Up,
            VirtualKey.PageDown,
            VirtualKey.PageUp,
            VirtualKey.NavigationDown,
            VirtualKey.NavigationUp 
        };

        public ICommand IndexKeyCommand
        {
            get => (ICommand)GetValue(IndexKeyCommandProperty);
            set => SetValue(IndexKeyCommandProperty, value);
        }

        public static readonly DependencyProperty IndexKeyCommandProperty =
            DependencyProperty.Register(nameof(IndexKeyCommand), typeof(ICommand), typeof(AlbumShortcutsBehavior), new PropertyMetadata(null));

        
        public Control MainNavigationControl
        {
            get => (Control)GetValue(MainNavigationControlProperty);
            set => SetValue(MainNavigationControlProperty, value);
        }

        public static readonly DependencyProperty MainNavigationControlProperty =
            DependencyProperty.Register(nameof(MainNavigationControl), typeof(Control), typeof(AlbumShortcutsBehavior), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            base.OnAttached();
            CoreWindow.GetForCurrentThread().KeyDown -= OnKeyDown;
            CoreWindow.GetForCurrentThread().KeyDown += OnKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            CoreWindow.GetForCurrentThread().KeyDown -= OnKeyDown;
        }

        private void OnKeyDown(CoreWindow coreWindow, KeyEventArgs args)
        {
            if (IndexKeyCommand == null)
                return;

            object focusedControl = FocusManager.GetFocusedElement();
            if (focusedControl is TextBox)
                return;

            string keyString = args.VirtualKey.ToString();
            string potentialNumberString = keyString.Replace("Number", "").Replace("Pad", "");

            if (int.TryParse(potentialNumberString, out int indexNumber) && indexNumber > 0 && indexNumber < 10)
            {
                if (IndexKeyCommand.CanExecute(indexNumber))
                    IndexKeyCommand.Execute(indexNumber);
            }
        }
    }
}