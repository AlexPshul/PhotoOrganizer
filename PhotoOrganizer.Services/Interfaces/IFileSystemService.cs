using System.Threading.Tasks;

namespace PhotoOrganizer.Services.Interfaces
{
    public interface IFileSystemService
    {
        Task<bool> DoesDirectoryExists(string path);
        Task<bool> IsValidDirectory(string destinationPath);
    }
}