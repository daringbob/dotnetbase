using System.Collections.Generic;
using src.Models;

namespace src.Repositories.Store
{
    public interface IStoreRepository
    {

        bool CheckExists(string serverRelativeUrl);
        Task<StoreRecord> CreateFile(string serverRelativeUrl, string fileName, int? refID, string? dataSource, Stream stream);
        StoreRecord GetFileInfo(string serverRelativeUrl);
        Task DeleteFile(string serverRelativeUrl);
    }
}
