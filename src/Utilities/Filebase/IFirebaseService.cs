using System.Collections.Generic;
using Firebase.Auth;
using src.Models;

namespace src.Utilities.Filebase
{
    public interface IFirebaseService
    {
        Task<string> UploadToFirebaseAsync(string serverRelativeUrl, Stream stream);
        Task<string> CreateFolderInFirebaseAsync(string folderPath);

        Task DeleteFileFromFirebaseAsync(string filePath);
        Task<bool> CheckIfPathExistsInFirebaseAsync(string path);
    }
}
