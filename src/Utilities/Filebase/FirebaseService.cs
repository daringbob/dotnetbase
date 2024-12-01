using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace src.Utilities.Filebase
{
    public class FirebaseService : IFirebaseService
    {
        private readonly IConfiguration _configuration;
        private Task<UserCredential> _userCredentialTask;

        public FirebaseService(IConfiguration configuration)
        {
            _configuration = configuration;
            _userCredentialTask = InitializeFirebaseAsync();
        }

        // Initialize and authenticate Firebase user
        private async Task<UserCredential> InitializeFirebaseAsync()
        {
            // Setup Firebase Auth configuration
            var config = new FirebaseAuthConfig
            {
                ApiKey = _configuration["Firebase:ApiKey"],
                AuthDomain = _configuration["Firebase:AuthDomain"],
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };

            var client = new FirebaseAuthClient(config);

            // Authenticate the user and cache the credential
            try
            {
                var userCredential = await client.SignInWithEmailAndPasswordAsync(
                  _configuration["Firebase:Email"],
                  _configuration["Firebase:Password"]
              );

                return userCredential;
            }
            catch (System.Exception)
            {
                return null;
            }

        }

        // Retrieve the user credential (cached)
        private async Task<UserCredential> GetUserCredentialAsync()
        {
            // Await the cached user credential task
            return await _userCredentialTask;
        }

        // Upload file to Firebase Storage
        public async Task<string> UploadToFirebaseAsync(string serverRelativeUrl, Stream stream)
        {
            // Get the cached user credential
            var userCredential = await GetUserCredentialAsync();

            // Get Firebase token
            var firebaseToken = await userCredential.User.GetIdTokenAsync();

            // Set up Firebase Storage options with the token
            var cancellationToken = new CancellationTokenSource();
            var token = await userCredential.User.GetIdTokenAsync();
            List<string> path = serverRelativeUrl.Split('/').Where(p => p != "").ToList();
            var firebaseStorage = new FirebaseStorage(
            _configuration["Firebase:StorageBucket"], // The Firebase storage bucket
            new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(token), // Use token for auth
                ThrowOnCancel = true // Handle cancellation
            }
            ).Child(serverRelativeUrl);


            try
            {
                // Wait for the upload to complete and return the download URL
                string downloadUrl = await firebaseStorage.PutAsync(stream, cancellationToken.Token);

                return downloadUrl;
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., upload failure)
                throw new Exception($"Error uploading file: {ex.Message}", ex);
            }
        }

        public async Task<string> CreateFolderInFirebaseAsync(string folderPath)
        {
            // Get the cached user credential
            var userCredential = await GetUserCredentialAsync();

            // Get Firebase token
            var firebaseToken = await userCredential.User.GetIdTokenAsync();

            // Set up Firebase Storage options with the token
            var token = await userCredential.User.GetIdTokenAsync();
            var firebaseStorage = new FirebaseStorage(
            _configuration["Firebase:StorageBucket"], // The Firebase storage bucket
            new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(token), // Use token for auth
                ThrowOnCancel = true // Handle cancellation
            }
            ).Child(folderPath);


            try
            {
                // Create an empty file to represent the folder
                string url = await firebaseStorage.PutAsync(new MemoryStream(new byte[0]));
                Console.WriteLine($"Folder created: {url}");
                // No download URL for folder creation, return success message
                return url;
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., folder creation failure)
                throw new Exception($"Error creating folder: {ex.Message}", ex);
            }
        }

        public async Task<FirebaseMetaData> GetMetaDataAsync(string path)
        {
            // Get the cached user credential
            var userCredential = await GetUserCredentialAsync();

            // Get Firebase token
            var firebaseToken = await userCredential.User.GetIdTokenAsync();

            // Set up Firebase Storage options with the token
            var token = await userCredential.User.GetIdTokenAsync();
            var firebaseStorage = new FirebaseStorage(
            _configuration["Firebase:StorageBucket"], // The Firebase storage bucket
            new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(token), // Use token for auth
                ThrowOnCancel = true // Handle cancellation
            }
            ).Child(path);



            try
            {
                // Try to get metadata of the file/folder
                var metaData = await firebaseStorage.GetMetaDataAsync();
                return metaData;
            }
            catch (Exception ex)
            {
                // If an exception occurs, it means the path does not exist
                return null;
            }
        }

        public async Task<bool> CheckIfPathExistsInFirebaseAsync(string path)
        {
            // Get the cached user credential
            var userCredential = await GetUserCredentialAsync();

            // Get Firebase token
            var firebaseToken = await userCredential.User.GetIdTokenAsync();

            // Set up Firebase Storage options with the token
            var token = await userCredential.User.GetIdTokenAsync();
            var firebaseStorage = new FirebaseStorage(
            _configuration["Firebase:StorageBucket"], // The Firebase storage bucket
            new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(token), // Use token for auth
                ThrowOnCancel = true // Handle cancellation
            }
            ).Child(path);



            try
            {
                // Try to get metadata of the file/folder
                var dowwnloadUrl = await firebaseStorage.GetMetaDataAsync();
                return dowwnloadUrl != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking path: {ex.Message}");
                // If an exception occurs, it means the path does not exist
                return false;
            }
        }

        public async Task DeleteFileFromFirebaseAsync(string filePath)
        {
            // Get the cached user credential
            var userCredential = await GetUserCredentialAsync();

            // Get Firebase token
            var firebaseToken = await userCredential.User.GetIdTokenAsync();

            // Set up Firebase Storage options with the token
            var token = await userCredential.User.GetIdTokenAsync();
            var firebaseStorage = new FirebaseStorage(
            _configuration["Firebase:StorageBucket"], // The Firebase storage bucket
            new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(token), // Use token for auth
                ThrowOnCancel = true // Handle cancellation
            }
            ).Child(filePath);


            try
            {
                // Delete the file
                await firebaseStorage.DeleteAsync();
                Console.WriteLine($"File deleted: {filePath}");
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., deletion failure)
                throw new Exception($"Error deleting file: {ex.Message}", ex);
            }
        }


    }
}
