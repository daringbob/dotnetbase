using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json; // Make sure to include this for JsonConvert
using src.Dtos.Auth;

public static class GraphApiHelper
{
    public static async Task<Office365UserInfo> GetUserInfoAsync(string accessToken)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                accessToken
            );

            string requestUrl = "https://graph.microsoft.com/v1.0/me";

            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Office365UserInfo userInfo = JsonConvert.DeserializeObject<Office365UserInfo>(
                    jsonResponse
                )!;
                return userInfo;
            }
            else
            {
                throw new Exception($"Failed to retrieve user info: {response.StatusCode}");
            }
        }
    }
}
