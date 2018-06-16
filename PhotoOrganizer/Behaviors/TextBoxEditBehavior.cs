using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;

namespace PhotoOrganizer.Behaviors
{
    public class TextBoxEditBehavior : Behavior<TextBox>
    {
        private string _initialValue;
        
        public ICommand EditCompletedCommand
        {
            get => (ICommand)GetValue(EditCompletedCommandProperty);
            set => SetValue(EditCompletedCommandProperty, value);
        }

        public static readonly DependencyProperty EditCompletedCommandProperty =
            DependencyProperty.Register(nameof(EditCompletedCommand), typeof(ICommand), typeof(TextBoxEditBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotFocus += OnGotFocus;
            AssociatedObject.KeyUp += OnKeyUp;
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            _initialValue = AssociatedObject.Text;
            AssociatedObject.LostFocus -= OnLostFocus;
            AssociatedObject.LostFocus += OnLostFocus;
        }

        private void OnKeyUp(object sender, KeyRoutedEventArgs keyRoutedEventArgs)
        {
            if (keyRoutedEventArgs.Key == VirtualKey.Enter)
                LoseFocus();
            else if (keyRoutedEventArgs.Key == VirtualKey.Escape)
            {
                AssociatedObject.LostFocus -= OnLostFocus;
                AssociatedObject.Text = _initialValue;
                LoseFocus();
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (EditCompletedCommand?.CanExecute(AssociatedObject.Text) ?? false)
                EditCompletedCommand?.Execute(AssociatedObject.Text);
        }

        private void LoseFocus()
        {
            AssociatedObject.IsTabStop = false;
            AssociatedObject.IsEnabled = false;
            AssociatedObject.IsEnabled = true;
            AssociatedObject.IsTabStop = true;
        }
    }
}