using System.Collections.Generic;
using Firebase.Auth;
using src.Models;

namespace src.Utilities.Filebase
{
    public interface IFirebaseService
    {
        Task<string> UploadToFirebaseAsync(string serverRelativeUrl, Stream stream);
    }
}
