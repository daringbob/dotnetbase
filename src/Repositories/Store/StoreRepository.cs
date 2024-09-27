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



        private async Task CreateRootRecordAsync()
        {
            var storeRecord = await _context.StoreRecords.FirstOrDefaultAsync(r => r.ServerRelativeUrl == _rootServerRelativeUrl);
            if (storeRecord == null)
            {
                storeRecord = new StoreRecord
                {
                    Name = "Store",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    ParentID = null,
                    ServerRelativeUrl = _rootServerRelativeUrl,
                    IsFolder = true
                };
                await _context.StoreRecords.AddAsync(storeRecord);
                await _context.SaveChangesAsync();
            }

            var imageStoreRecord = await _context.StoreRecords.FirstOrDefaultAsync(r => r.ServerRelativeUrl == _imageStoreServerRelativeUrl);

            if (imageStoreRecord == null)
            {
                imageStoreRecord = new StoreRecord
                {
                    Name = "ImageStore",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    ParentID = storeRecord.Id,
                    ServerRelativeUrl = _imageStoreServerRelativeUrl,
                    IsFolder = true
                };
                await _context.StoreRecords.AddAsync(imageStoreRecord);
                await _context.SaveChangesAsync();
            }
            var questionListImageRecord = await _context.StoreRecords.FirstOrDefaultAsync(r => r.ServerRelativeUrl == _questionListImageServerRelativeUrl);

            if (questionListImageRecord == null)
            {
                questionListImageRecord = new StoreRecord
                {
                    Name = "QuestionListImage",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    ParentID = storeRecord.Id,
                    ServerRelativeUrl = _questionListImageServerRelativeUrl,
                    IsFolder = true
                };
                await _context.StoreRecords.AddAsync(questionListImageRecord);
                await _context.SaveChangesAsync();
            }
        }


        public async Task InitStore()
        {
            if (!Directory.Exists(_rootServerRelativeUrl))
            {
                Directory.CreateDirectory(_rootServerRelativeUrl);
            }
            if (!Directory.Exists(_imageStoreServerRelativeUrl))
            {
                Directory.CreateDirectory(_imageStoreServerRelativeUrl);
            }
            if (!Directory.Exists(_questionListImageServerRelativeUrl))
            {
                Directory.CreateDirectory(_questionListImageServerRelativeUrl);
            }



            await CreateRootRecordAsync();
        }

        public bool CheckExists(string serverRelativeUrl)
        {
            return _context.StoreRecords.Any(r => r.ServerRelativeUrl == serverRelativeUrl);
        }

        public async Task<StoreRecord> CreateFile(string serverRelativeUrl, string fileName, int? refID, string? dataSource, Stream stream)
        {


            // Upload the file to Firebase
            // string firebaseFileUrl = await UploadToFirebaseAsync(fileBytes, fileName);

            var fileFullPath = serverRelativeUrl.TrimStart('/') + "/" + fileName;
            if (serverRelativeUrl != "/")
            {
                fileFullPath = serverRelativeUrl + "/" + fileName;
            }


            StoreRecord storeRecord = await GetStoreByServerRelativeUrl(fileFullPath);
            // if (storeRecord != null)
            // {
            //     return storeRecord;
            // }

            StoreRecord parentRecord = await GetStoreByServerRelativeUrl(serverRelativeUrl);
            Console.WriteLine(fileFullPath);
            // if (parentRecord == null && serverRelativeUrl != "/")
            // {
            //     throw new DirectoryNotFoundException($"Directory not found: {serverRelativeUrl}");
            // }
            try
            {
                string downloadUrl = await _firebaseService.UploadToFirebaseAsync(fileFullPath, stream);
                storeRecord = new StoreRecord
                {
                    Name = fileName,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    RefID = refID,
                    ParentID = parentRecord?.Id,
                    DataSource = dataSource,
                    ServerRelativeUrl = fileFullPath,
                    downloadUrl = downloadUrl,
                    IsFolder = false
                };
                _context.StoreRecords.Add(storeRecord);
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
        public string CreateFolderIfNotExists(string serverRelativeUrl, int rootParentID, int? refID, string? dataSource)
        {
            var folders = serverRelativeUrl.Replace('/', '\\').Split('\\');
            var currentPath = _rootServerRelativeUrl;
            int parentID = rootParentID;

            foreach (var folder in folders)
            {
                currentPath = Path.Combine(currentPath, folder);

                if (!CheckExists(currentPath))
                {
                    Directory.CreateDirectory(currentPath);

                    var storeRecord = new StoreRecord
                    {
                        Name = folder,
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        RefID = refID,
                        ParentID = parentID,
                        ServerRelativeUrl = currentPath,
                        DataSource = dataSource,
                        IsFolder = true
                    };
                    _context.StoreRecords.Add(storeRecord);
                    _context.SaveChanges();

                    parentID = storeRecord.Id;
                }
                else
                {
                    var existingRecord = _context.StoreRecords.FirstOrDefault(r => r.ServerRelativeUrl == currentPath);
                    if (existingRecord != null)
                    {
                        parentID = existingRecord.Id;
                    }
                }
            }

            return currentPath;
        }

        private int GetParentIdForPath(string serverRelativeUrl)
        {
            var record = _context.StoreRecords.FirstOrDefault(r => r.ServerRelativeUrl == serverRelativeUrl);
            return record.Id;
        }

        public StoreRecord CreateFolder(string serverRelativeUrl, int? refID, string? dataSource)
        {
            var rootRecord = _context.StoreRecords.FirstOrDefault(r => r.ServerRelativeUrl == _rootServerRelativeUrl);
            var folderPath = CreateFolderIfNotExists(serverRelativeUrl, rootRecord.Id, refID, dataSource);
            return _context.StoreRecords.FirstOrDefault(r => r.ServerRelativeUrl == folderPath);
        }

        public byte[] GetFileContent(string serverRelativeUrl)
        {
            if (CheckExists(serverRelativeUrl))
            {
                return File.ReadAllBytes(serverRelativeUrl);
            }
            else
            {
                throw new FileNotFoundException($"File not found, {serverRelativeUrl}");
            }
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

        public List<StoreRecord> GetFolderInfo(string serverRelativeUrl, int? refID, string? dataSource)
        {
            if (CheckExists(serverRelativeUrl))
            {
                var query = _context.StoreRecords.Where(r => r.ServerRelativeUrl.StartsWith(serverRelativeUrl));

                if (refID.HasValue)
                {
                    query = query.Where(r => r.RefID == refID.Value);
                }

                if (!string.IsNullOrEmpty(dataSource))
                {
                    query = query.Where(r => r.DataSource == dataSource);
                }

                return query.ToList();
            }
            else
            {
                throw new DirectoryNotFoundException($"Directory not found: {serverRelativeUrl}");
            }
        }

        public void DeleteFile(string serverRelativeUrl)
        {
            if (File.Exists(serverRelativeUrl))
            {
                File.Delete(serverRelativeUrl);

                var record = _context.StoreRecords.FirstOrDefault(r => r.ServerRelativeUrl == serverRelativeUrl);
                if (record != null)
                {
                    _context.StoreRecords.Remove(record);
                    _context.SaveChanges();
                }
            }
            else
            {
                throw new FileNotFoundException($"File not found, {serverRelativeUrl}");
            }
        }

        public void DeleteFolder(string serverRelativeUrl)
        {
            if (Directory.Exists(serverRelativeUrl))
            {
                Directory.Delete(serverRelativeUrl, true);
                var records = _context.StoreRecords
                    .Where(r => r.ServerRelativeUrl.StartsWith(serverRelativeUrl))
                    .ToList();

                _context.StoreRecords.RemoveRange(records);
                _context.SaveChanges();
            }
            else
            {
                throw new DirectoryNotFoundException($"Directory not found: {serverRelativeUrl}");
            }
        }

        public async Task<string> UploadTest(Stream stream, string fileName)
        {
            var cancellationToken = new CancellationTokenSource();
            var config = new FirebaseAuthConfig
            {
                ApiKey = _configuration["Firebase:ApiKey"],
                AuthDomain = _configuration["Firebase:AuthDomain"],
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                    // ...
                }
            };
            var client = new FirebaseAuthClient(config);
            var userCredential = await client.SignInWithEmailAndPasswordAsync(_configuration["Firebase:Email"], _configuration["Firebase:Password"]);
            var task = new FirebaseStorage(_configuration["Firebase:StorageBucket"], new FirebaseStorageOptions { AuthTokenAsyncFactory = () => Task.FromResult(userCredential.User.GetIdTokenAsync().Result), ThrowOnCancel = true })
               .Child("uploads")
               .Child(fileName)
               .PutAsync(stream, cancellationToken.Token);
            try
            {
                string downloadUrl = await task;
                return downloadUrl;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
