using noeTaskManager_app.Models.AuthModels;
using noeTaskManager_app.Services.Interfaces;
using System.Security.Policy;
using System.Text;
using System.Text.Json;

namespace noeTaskManager_app.Services
{
    public class AuthService: IAuthService
    {
        protected string _serverUrl;
        protected HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serverUrl = "http://localhost:5000";
        }

        
        public async Task<SigninResponseObject> SignIn(string email, string password)
        {
            try
            {
                var endpoint = $"{_serverUrl}/signin";
                var creds = new
                {
                    Email = email,
                    Password = password
                };

                var serilizedObject = JsonSerializer.Serialize(creds);
                var data = new StringContent(serilizedObject, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, data);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                var deserilisedBody = JsonSerializer.Deserialize<SigninResponseObject>(responseBody);

                return deserilisedBody;

            } catch(Exception e)
            {
                throw new Exception($"Error signing user in: {e}");
            }
        }

        public async Task<bool> SignUp(string firstName, string lastName, string email, string password)
        {
            try
            {
                await;
                return true;
            }
        }

    }
}
