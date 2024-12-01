using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Storage;
using src.Models;

namespace src.Utilities.Filebase
{
    public interface IFirebaseService
    {
        Task<string> UploadToFirebaseAsync(string serverRelativeUrl, Stream stream);
        Task<string> CreateFolderInFirebaseAsync(string folderPath);

        Task<FirebaseMetaData> GetMetaDataAsync(string path);
        Task DeleteFileFromFirebaseAsync(string filePath);
        Task<bool> CheckIfPathExistsInFirebaseAsync(string path);
    }
}
