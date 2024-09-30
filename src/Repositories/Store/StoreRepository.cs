using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using src.Models;
using src.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using Firebase.Storage;
using Firebase.Auth.Providers;
using Firebase.Auth;
using src.Utilities.Filebase;

namespace src.Repositories.Store
{
    public class StoreRepository : IStoreRepository
    {
        private readonly string _rootServerRelativeUrl = "Store";
        private readonly string _imageStoreServerRelativeUrl = "Store\\ImageStore";
        private readonly string _questionListImageServerRelativeUrl = "Store\\QuestionListImage";
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IFirebaseService _firebaseService;
        public StoreRepository(AppDbContext context, IConfiguration configuration, IFirebaseService firebaseService)
        {
            _context = context;
            _configuration = configuration;
            _firebaseService = firebaseService;
        }


        public bool CheckExists(string serverRelativeUrl)
        {
            return _context.StoreRecords.Any(r => r.ServerRelativeUrl == serverRelativeUrl && !r.IsFolder);
        }

        public async Task<StoreRecord> CreateFile(string serverRelativeUrl, string fileName, int? refID, string? dataSource, Stream stream)
        {
            var fileFullPath = serverRelativeUrl.TrimStart('/') + "/" + fileName;
            if (serverRelativeUrl != "/")
            {
                fileFullPath = serverRelativeUrl + "/" + fileName;
            }

            StoreRecord storeRecord = await GetStoreByServerRelativeUrl(fileFullPath);
            if (storeRecord != null)
            {
                return storeRecord;
            }

            StoreRecord parentRecord = await GetStoreByServerRelativeUrl(serverRelativeUrl);

            if (parentRecord == null && serverRelativeUrl != "/")
            {
                throw new DirectoryNotFoundException($"Directory not found: {serverRelativeUrl}");
            }
            try
            {
                string downloadUrl = await _firebaseService.UploadToFirebaseAsync(fileFullPath.TrimStart('/'), stream);
                storeRecord = new StoreRecord
                {
                    Name = fileName,
                    RefID = refID,
                    ParentID = parentRecord?.Id,
                    DataSource = dataSource,
                    ServerRelativeUrl = fileFullPath,
                    downloadUrl = downloadUrl,
                    IsFolder = false
                };
                await _context.StoreRecords.AddAsync(storeRecord);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during upload: {ex.Message}");
                throw;  // or handle it accordingly
            }

            return storeRecord;
        }

        private async Task<StoreRecord> GetStoreByServerRelativeUrl(string serverRelativeUrl)
        {
            return await _context.StoreRecords.FirstOrDefaultAsync(r => r.ServerRelativeUrl == serverRelativeUrl);
        }

        public StoreRecord GetFileInfo(string serverRelativeUrl)
        {
            if (CheckExists(serverRelativeUrl))
            {
                return _context.StoreRecords.FirstOrDefault(r => r.ServerRelativeUrl == serverRelativeUrl);
            }
            else
            {
                throw new FileNotFoundException($"File not found, {serverRelativeUrl}");
            }
        }

        public async Task DeleteFile(string serverRelativeUrl)
        {
            StoreRecord record = await GetStoreByServerRelativeUrl(serverRelativeUrl);
            bool isExsist = await _firebaseService.CheckIfPathExistsInFirebaseAsync(serverRelativeUrl.TrimStart('/'));
            if (record != null && !record.IsFolder && isExsist)
            {
                await _firebaseService.DeleteFileFromFirebaseAsync(serverRelativeUrl.TrimStart('/'));
                _context.StoreRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new FileNotFoundException($"File not found, {serverRelativeUrl}");
            }


        }




    }
}
