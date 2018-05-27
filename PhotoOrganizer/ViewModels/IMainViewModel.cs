using System.Collections.ObjectModel;

namespace PhotoOrganizer.ViewModels
{
    public interface IMainViewModel
    {
        ObservableCollection<string> AvailableAlbums { get; }
    }
}