using PhotoOrganizer.ViewModels;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PhotoOrganizer.Views
{
    public sealed partial class AlbumView : IViewFor<IAlbumViewModel>
    {
        public AlbumView()
        {
            InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IAlbumViewModel)value;
        }

        public IAlbumViewModel ViewModel { get; set; }
    }
}