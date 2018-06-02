using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;

namespace PhotoOrganizer.BehaviorActions
{
    public class ItemDoubleTapExecuteCommandAction : DependencyObject, IAction
    {
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ItemDoubleTapExecuteCommandAction), new PropertyMetadata(null));

        public object Execute(object sender, object parameter)
        {
            if (Command == null)
                return false;

            if (!(parameter is DoubleTappedRoutedEventArgs doubleTappedRoutedEventArgs))
                return false;

            if (!(doubleTappedRoutedEventArgs.OriginalSource is DependencyObject originalSource))
                return false;

            SelectorItem selectorItemParent = GetSelectorItemParent(originalSource);
            if (selectorItemParent == null)
                return false;

            if (Command.CanExecute(selectorItemParent.Content))
                Command.Execute(selectorItemParent.Content);

            return true;
        }

        private SelectorItem GetSelectorItemParent(DependencyObject dependencyObject)
        {
            if (dependencyObject is SelectorItem selectorItem)
                return selectorItem;

            DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent is null || parent is Selector)
                return null;

            return GetSelectorItemParent(parent);
        }
    }
}