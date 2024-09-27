using System.Collections.Generic;
using src.Models;

namespace src.Repositories.Store
{
    public interface IStoreRepository
    {

        Task InitStore();
        bool CheckExists(string serverRelativeUrl);
        Task<StoreRecord> CreateFile(string serverRelativeUrl, string fileName, int? refID, string? dataSource, Stream stream);
        string CreateFolderIfNotExists(string serverRelativeUrl, int rootRefId, int? refID, string? dataSource);
        StoreRecord CreateFolder(string serverRelativeUrl, int? refID, string? dataSource);
        byte[] GetFileContent(string serverRelativeUrl);
        StoreRecord GetFileInfo(string serverRelativeUrl);
        List<StoreRecord> GetFolderInfo(string serverRelativeUrl, int? refID, string? dataSource);
        void DeleteFile(string serverRelativeUrl);
        void DeleteFolder(string serverRelativeUrl);
        public Task<string> UploadTest(Stream stream, string fileName);
    }
}
