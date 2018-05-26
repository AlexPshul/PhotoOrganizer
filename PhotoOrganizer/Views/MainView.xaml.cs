// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using PhotoOrganizer.ViewModels;
using ReactiveUI;

namespace PhotoOrganizer.Views
{
    public sealed partial class MainView : IViewFor<IMainViewModel>
    {
        public MainView()
        {
            InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set
            {
                ViewModel = (IMainViewModel)value;
                DataContext = ViewModel;
            }
        }

        public IMainViewModel ViewModel { get; set; }
    }
}