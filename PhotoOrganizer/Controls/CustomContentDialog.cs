using System;
using System.Reactive.Linq;
using System.Reactive.Windows.Foundation;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace PhotoOrganizer.Controls
{
    public class CustomContentDialog : ContentDialog
    {
        private TaskCompletionSource<ContentDialogResult> _resultCompletionSource = new TaskCompletionSource<ContentDialogResult>();

        public new async Task<ContentDialogResult> ShowAsync()
        {
            base.ShowAsync();
            _resultCompletionSource = new TaskCompletionSource<ContentDialogResult>();
            ContentDialogResult contentDialogResult = await _resultCompletionSource.Task;

            Hide();

            return contentDialogResult;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button primaryButton = (Button)GetTemplateChild("Button1");
            Button secondaryButton = (Button)GetTemplateChild("Button2");

            primaryButton.Click += PrimaryButtonOnClick;
            secondaryButton.Click += SecondaryButtonOnClick;

            PreviewKeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyRoutedEventArgs keyRoutedEventArgs)
        {
            keyRoutedEventArgs.Handled = true;

            if (keyRoutedEventArgs.Key != VirtualKey.Escape)
                return;

            _resultCompletionSource.SetResult(ContentDialogResult.None);
        }

        private void PrimaryButtonOnClick(object sender, RoutedEventArgs e) => _resultCompletionSource.SetResult(ContentDialogResult.Primary);
        private void SecondaryButtonOnClick(object sender, RoutedEventArgs e) => _resultCompletionSource.SetResult(ContentDialogResult.Secondary);
    }
}