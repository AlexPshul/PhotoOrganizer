using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace PhotoOrganizer.BehaviorActions
{
    public class CancelOnLastElementReorderAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            if (!(sender is ListViewBase listViewBase) || 
                !(parameter is DragItemsStartingEventArgs dragArgs))
                return false;

            object lastItem = listViewBase.Items?[listViewBase.Items.Count - 1];
            if (lastItem == null)
                return false;

            dragArgs.Cancel = dragArgs.Items?.Contains(lastItem) ?? false;

            return true;
        }
    }
}