using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using PhotoOrganizer.ViewModels;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PhotoOrganizer.Views
{
    public sealed partial class GroupFolderView : IViewFor<IGroupFolderViewModel>
    {
        public GroupFolderView()
        {
            this.InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IGroupFolderViewModel)value;
        }
        public IGroupFolderViewModel ViewModel { get; set; }

        private void GroupNameTextBox_OnPreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                GroupNameTextBox.IsTabStop = false;
                GroupNameTextBox.IsEnabled = false;
                GroupNameTextBox.IsEnabled = true;
                GroupNameTextBox.IsTabStop = true;
            }
        }
    }
}