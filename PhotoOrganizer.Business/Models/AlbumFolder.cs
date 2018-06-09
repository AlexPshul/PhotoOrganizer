using System.Collections.Generic;

namespace PhotoOrganizer.Business.Models
{
    public class AlbumFolder
    {
        public string FullPath { get; }
        public IReadOnlyCollection<string> Images { get; }

        public AlbumFolder(string fullPath, IReadOnlyCollection<string> images)
        {
            FullPath = fullPath;
            Images = images;
        }
    }
}