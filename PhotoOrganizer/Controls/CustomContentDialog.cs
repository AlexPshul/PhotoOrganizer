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
        #region Private Members

        private TaskCompletionSource<ContentDialogResult> _resultCompletionSource = new TaskCompletionSource<ContentDialogResult>();

        #endregion

        #region Properties

        #region MessageFirstPart

        public string MessageFirstPart
        {
            get => (string)GetValue(MessageFirstPartProperty);
            set => SetValue(MessageFirstPartProperty, value);
        }

        public static readonly DependencyProperty MessageFirstPartProperty =
            DependencyProperty.Register(nameof(MessageFirstPart), typeof(string), typeof(CustomContentDialog), new PropertyMetadata(null));

        #endregion

        #region MessageSecondPart

        public string MessageSecondPart
        {
            get => (string)GetValue(MessageSecondPartProperty);
            set => SetValue(MessageSecondPartProperty, value);
        }

        public static readonly DependencyProperty MessageSecondPartProperty =
            DependencyProperty.Register(nameof(MessageSecondPart), typeof(string), typeof(CustomContentDialog), new PropertyMetadata(null));

        #endregion

        #region Parameter

        public string Parameter
        {
            get => (string)GetValue(ParameterProperty);
            set => SetValue(ParameterProperty, value);
        }

        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register(nameof(Parameter), typeof(string), typeof(CustomContentDialog), new PropertyMetadata(null));

        #endregion

        #endregion

        #region Constructors

        public CustomContentDialog(string messageTemplate, string parameter)
        {
            string[] questionPart = messageTemplate.Split("{0}");
            MessageFirstPart = questionPart[0];
            MessageSecondPart = questionPart[1];
            Parameter = parameter;
        }

        #endregion

        #region Public Methods

        public new async Task<ContentDialogResult> ShowAsync()
        {
            base.ShowAsync();
            _resultCompletionSource = new TaskCompletionSource<ContentDialogResult>();
            ContentDialogResult contentDialogResult = await _resultCompletionSource.Task;

            Hide();

            return contentDialogResult;
        }

        #endregion

        #region Protected Methods

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button primaryButton = (Button)GetTemplateChild("Button1");
            Button secondaryButton = (Button)GetTemplateChild("Button2");

            primaryButton.Click += PrimaryButtonOnClick;
            secondaryButton.Click += SecondaryButtonOnClick;

            PreviewKeyDown += OnKeyDown;
        }

        #endregion

        #region Private Methods

        private void OnKeyDown(object sender, KeyRoutedEventArgs keyRoutedEventArgs)
        {
            keyRoutedEventArgs.Handled = true;

            if (keyRoutedEventArgs.Key != VirtualKey.Escape)
                return;

            _resultCompletionSource.SetResult(ContentDialogResult.None);
        }

        private void PrimaryButtonOnClick(object sender, RoutedEventArgs e) => _resultCompletionSource.SetResult(ContentDialogResult.Primary);
        private void SecondaryButtonOnClick(object sender, RoutedEventArgs e) => _resultCompletionSource.SetResult(ContentDialogResult.Secondary);

        #endregion
    }
}