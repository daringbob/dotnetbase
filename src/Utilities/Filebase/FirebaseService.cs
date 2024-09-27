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
            var userCredential = await client.SignInWithEmailAndPasswordAsync(
                _configuration["Firebase:Email"],
                _configuration["Firebase:Password"]
            );

            return userCredential;
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
                ).Child(path[0]);

            // Add the remaining path segments
            path.RemoveAt(0);
            foreach (var p in path)
            {
                firebaseStorage.Child(p);
                Console.WriteLine($"Child: {p}");
            }

            // path.ForEach(p => firebaseStorage.Child(p));

            try
            {
                // Wait for the upload to complete and return the download URL
                string downloadUrl = await firebaseStorage.PutAsync(stream, cancellationToken.Token);
                Console.WriteLine($"File uploaded to: {downloadUrl}");
                return downloadUrl;
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., upload failure)
                throw new Exception($"Error uploading file: {ex.Message}", ex);
            }
        }
    }
}
