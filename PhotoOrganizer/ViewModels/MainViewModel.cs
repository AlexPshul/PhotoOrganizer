using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using PhotoOrganizer.Business.Interfaces;
using PhotoOrganizer.Business.Models;
using PhotoOrganizer.Controls;
using PhotoOrganizer.Infrastructure;
using PhotoOrganizer.Text;
using PhotoOrganizer.UIInfrastructure;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    internal class MainViewModel : ReactiveObject, IMainViewModel
    {
        private object _currentViewModel;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
        }

        public MainViewModel(Func<IAlbumSelectViewModel> albumSelectViewModelFunc, 
                             Func<IAlbumViewModel> albumViewModelFunc,
                             ICurrentAlbumManager currentAlbumManager)
        {
            CurrentViewModel = albumSelectViewModelFunc();

            currentAlbumManager.AlbumOpened.Subscribe(_ => CurrentViewModel = albumViewModelFunc());
            currentAlbumManager.AlbumClosed.Subscribe(_ => CurrentViewModel = albumSelectViewModelFunc());
        }
    }
}