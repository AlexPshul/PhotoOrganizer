using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace PhotoOrganizer.ViewModels
{
    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        private string _text;
        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }

        public MainViewModel()
        {
            Text = "This is going to be a cool photo editing application.";
        }
    }
}