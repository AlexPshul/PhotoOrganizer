using System.Collections.Generic;
using System.IO;
using PhotoOrganizer.Infrastructure;

namespace PhotoOrganizer.Services.DataObjects
{
    public class FolderData
    {
        public string FullPath { get; }
        public IReadOnlyCollection<string> Files { get; }

        public FolderData(string fullPath, IEnumerable<string> files)
        {
            FullPath = fullPath;
            Files = files as IReadOnlyCollection<string> ?? files.ToReadOnlyCollection();
        }
    }
}