using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoOrganizer.Services.Interfaces
{
    public interface ILocalStorageService
    {
        Task<string> GetString(string group, string key);
        Task CreateString(string group, string key, string value);
        Task DeleteString(string group, string key);

        Task<IReadOnlyCollection<string>> GetAvailableKeys(string group);
    }
}