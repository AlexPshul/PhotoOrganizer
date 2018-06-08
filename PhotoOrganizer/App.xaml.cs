using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PhotoOrganizer.Infrastructure;
using PhotoOrganizer.UIInfrastructure;
using PhotoOrganizer.Views;
using ReactiveUI;
using Splat;

namespace PhotoOrganizer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private readonly AutofacMutableDependencyResolver _resolver = new AutofacMutableDependencyResolver();
        
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            InitializeThemeColors();
            InitializeTitleBar();

            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;

            ApplicationView.PreferredLaunchViewSize = new Size(bounds.Width, bounds.Height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            Locator.Current = _resolver;
            Locator.CurrentMutable = _resolver;
            Locator.CurrentMutable.InitializeReactiveUI();

            _resolver.Build();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        private void InitializeTitleBar()
        {
            //var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            //// Set active window colors
            //titleBar.ForegroundColor = Resources["SecondaryForegroundColor"] as Color?;
            //titleBar.BackgroundColor = Resources["HalfAccentColor"] as Color?;
            //titleBar.ButtonForegroundColor = Resources["SecondaryForegroundColor"] as Color?;
            //titleBar.ButtonBackgroundColor = Resources["HalfAccentColor"] as Color?;
            //titleBar.ButtonHoverForegroundColor = Resources["SecondaryForegroundColor"] as Color?;
            //titleBar.ButtonHoverBackgroundColor = Resources["FullAccentColor"] as Color?;
            //titleBar.ButtonPressedForegroundColor = Resources["SecondaryForegroundColor"] as Color?;
            //titleBar.ButtonPressedBackgroundColor = Resources["FullAccentColor"] as Color?;

            //// Set inactive window colors
            //titleBar.InactiveForegroundColor = Resources["SecondaryForegroundColor"] as Color?;
            //titleBar.InactiveBackgroundColor = Resources["SmallAccentColor"] as Color?;
            //titleBar.ButtonInactiveForegroundColor = Resources["SecondaryForegroundColor"] as Color?;
            //titleBar.ButtonInactiveBackgroundColor = Resources["SmallAccentColor"] as Color?;
        }

        private void InitializeThemeColors()
        {
            Resources["SystemControlForegroundBaseLowHighBrush"] = Resources["MainForegroundBrush"];
            Resources["SystemControlForegroundBaseMediumHighBrush"] = Resources["SecondaryForegroundBrush"];
            Resources["SystemControlForegroundBaseHighHighBrush"] = Resources["SecondaryForegroundBrush"];

            Resources["SystemControlHighlightAltBaseHighBrush"] = Resources["SecondaryForegroundBrush"];
            Resources["SystemControlForegroundBaseHighBrush"] = Resources["MainForegroundBrush"];

            Resources["SystemControlHighlightListLowBrush"] = Resources["MediumHighlightBrush"];

            Resources["SystemControlHighlightListAccentLowBrush"] = Resources["HighHighlightBrush"];
            Resources["SystemControlHighlightListAccentMediumBrush"] = Resources["HighHighlightBrush"];
            Resources["SystemControlHighlightListAccentHighBrush"] = Resources["MediumHighlightBrush"];
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
