using Windows.UI.Xaml.Controls;
using PhotoOrganizer.ViewModels;
using ReactiveUI;
using Splat;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PhotoOrganizer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                IMainViewModel mainViewModel = (IMainViewModel)Locator.Current.GetService(typeof(IMainViewModel));
                //Content = new ViewModelViewHost { ViewModel = mainViewModel };
            };
        }
    }
}