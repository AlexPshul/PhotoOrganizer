using System.Collections.Generic;

namespace PhotoOrganizer.ViewModels
{
    public interface IAlbumFolderViewModel
    {
        string RelativePath { get; }
        IReadOnlyCollection<string> Images { get; }
    }
}