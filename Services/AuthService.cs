using Microsoft.AspNetCore.Http;
using noeTaskManager_app.Models.AuthModels;
using noeTaskManager_app.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace noeTaskManager_app.Services
{
    public class AuthService: IAuthService
    {
        protected string _serverUrl;
        protected HttpClient _httpClient;
        protected IHttpContextAccessor _httpContextAccessor;

        public AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _serverUrl = "http://localhost:5241/api";
        }

        public void RegisterTokenCookie(string jwt)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1) //MUST ALWAYS MATCH TOKEN EXPIRATION
            };

            httpContext.Response.Cookies.Append("JWT", jwt, cookieOptions);
        }

        public string GetTokenCookie()
        {
            if( _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("JWT", out string token))
            {
                return token;
            } else
            {
                return null;
            }
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

                var serializedObject = JsonSerializer.Serialize(creds);
                var data = new StringContent(serializedObject, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, data);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };
                var deserialisedBody = JsonSerializer.Deserialize<SigninResponseObject>(responseBody, serializerOptions);

                if (deserialisedBody.AccessToken == null)
                {
                    return null;
                }
                else
                {
                    RegisterTokenCookie(deserialisedBody.AccessToken);

                    return deserialisedBody;
                }

            }
            catch (HttpRequestException e)
            {
                // Log and handle HTTP request errors specifically
                throw new Exception($"Potentially network or server error when signing in: {e.Message}", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Error signing user in: {e.Message}", e);
            }
        }

        public async Task<bool> SignUp(string firstName, string lastName, string email, string password)
        {
            try
            {
                var endpoint = $"{_serverUrl}/signup";
                var creds = new
                {   
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password
                };

                var serializedObject = JsonSerializer.Serialize(creds);
                var data = new StringContent(serializedObject, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, data);

                return true;

            } catch(Exception e)
            {
                throw new Exception($"Error signing user up: {e.Message}", e);
            }
        }

    }
}
