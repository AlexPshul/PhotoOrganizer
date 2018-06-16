using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using PhotoOrganizer.Infrastructure;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace PhotoOrganizer.Controls
{
    public sealed class StreamImage : Control
    {
        #region Private Members

        private IDisposable _eventSubscriptions;
        private readonly Subject<Unit> _sourceUpdatedSubject = new Subject<Unit>();
        private readonly CacheDictionary<string, BitmapImage> _cache = new CacheDictionary<string, BitmapImage>();

        #endregion

        #region Properties

        public Image Image { get; private set; }

        public int CacheLimit
        {
            get => _cache.Limit;
            set => _cache.Limit = value;
        }

        #endregion

        #region Dependency Properties

        #region IsLoading
        
        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(StreamImage), new PropertyMetadata(false));

        #endregion

        #region Source
        
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(string), typeof(StreamImage), new PropertyMetadata(null, ReloadImage));

        #endregion

        #endregion

        #region Constructors

        public StreamImage()
        {
            DefaultStyleKey = typeof(StreamImage);

            CacheLimit = 1;

            SizeChanged += (sender, args) =>
            {
                if (args.PreviousSize != Size.Empty)
                    ReloadImage();
            };

            Loaded += (sender, args) =>
            {
                _eventSubscriptions = _sourceUpdatedSubject
                    .Do(_ => InitializeSourceFromCache())
                    .Throttle(TimeSpan.FromMilliseconds(250))
                    .ObserveOnDispatcher()
                    .Subscribe(_ => ReloadImage());
            };

            Unloaded += (sender, args) => _eventSubscriptions?.Dispose();
        }

        #endregion

        #region Protected Methods

        private bool _isLoaded;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Image = (Image)GetTemplateChild("PART_Image");

            Image.Loaded += (sender, args) =>
            {
                _isLoaded = true;
                ReloadImage();
            };
        }

        #endregion

        #region Private Methods

        private static void ReloadImage(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StreamImage sender = d as StreamImage;
            sender?._sourceUpdatedSubject.OnNext(Unit.Default);
        }

        private async void ReloadImage()
        {
            if (!_isLoaded || Image == null || Source == null)
                return;

            if (_cache.ContainsKey(Source))
                return;

            using (DisposableExtensions.Create(() => IsLoading = true, () => IsLoading = false))
            using (IRandomAccessStream stream = await FileRandomAccessStream.OpenAsync(Source, FileAccessMode.Read))
            {
                BitmapImage bitmapImage = new BitmapImage { DecodePixelWidth = (int)ActualWidth + 20 };

                await bitmapImage.SetSourceAsync(stream);
                _cache[Source] = bitmapImage;
                Image.Source = bitmapImage;
            }
        }

        private void InitializeSourceFromCache()
        {
            Image.Source = _cache.TryGetValue(Source ?? "", out BitmapImage cachedImage) 
                ? cachedImage 
                : null;
        }

        #endregion
    }
}