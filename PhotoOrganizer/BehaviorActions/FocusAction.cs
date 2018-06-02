using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace PhotoOrganizer.BehaviorActions
{
    public class FocusAction : DependencyObject, IAction
    {
        public Control TargetControl
        {
            get => (Control)GetValue(FocusAction.TargetControlProperty);
            set => SetValue(FocusAction.TargetControlProperty, value);
        }

        public static readonly DependencyProperty TargetControlProperty = DependencyProperty.Register(
            nameof(TargetControl),
            typeof(Control),
            typeof(FocusAction),
            new PropertyMetadata(null));

        public object Execute(object sender, object parameter)
        {
            Control control;
            if (ReadLocalValue(FocusAction.TargetControlProperty) != DependencyProperty.UnsetValue)
                control = TargetControl;
            else if (sender is Control senderControl)
                control = senderControl;
            else
                return false;

            control.Focus(FocusState.Keyboard);

            if (control is TextBox textBox)
                textBox.Select(0, textBox.Text.Length);

            return true;
        }
    }
}