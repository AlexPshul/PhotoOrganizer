using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
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
        public IMainViewModel MainViewModel { get; } = (IMainViewModel)Locator.Current.GetService(typeof(IMainViewModel));

        public MainPage()
        {
            InitializeComponent();

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBarOnLayoutMetricsChanged;
            
            Window.Current.SetTitleBar(AppTitleBar);
            Window.Current.Activated += (sender, args) =>
            {
                string state = args.WindowActivationState == CoreWindowActivationState.Deactivated ? "Inactive" : "Active";
                VisualStateManager.GoToState(this, state, false);
            };
        }

        private void CoreTitleBarOnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args) => NameBorder.Height = sender.Height + 2;
    }
}