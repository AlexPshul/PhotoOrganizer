using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PhotoOrganizer.ViewModels;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PhotoOrganizer.Views
{
    public sealed partial class NewAlbumView : UserControl, IViewFor<INewAlbumViewModel>
    {
        public NewAlbumView()
        {
            this.InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (INewAlbumViewModel)value;
        }

        public INewAlbumViewModel ViewModel { get; set; }
    }
}
