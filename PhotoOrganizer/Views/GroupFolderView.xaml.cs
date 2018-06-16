using Windows.UI.Xaml.Controls;
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
    }
}
