using noeTaskManager_app.Models;
using noeTaskManager_app.Services.Interfaces;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace noeTaskManager_app.Services
{
    public class TaskManagerService: ITaskManagerService
    {
        protected string _serverUrl;
        protected HttpClient _httpClient;
        protected IHttpContextAccessor _httpContextAccessor;

        public TaskManagerService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _serverUrl = "http://localhost:5241/api/";
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool isSuccess, List<TaskItem>?)> GetAllTasks()
        {
            try
            {
                var endpoint = $"{_serverUrl}/GetTask/All";

                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };
                List<TaskItem>? deserializedBody = JsonSerializer.Deserialize<List<TaskItem>>(responseBody, serializerOptions);

                if (deserializedBody == null)
                {
                    return (false, null);
                }
                else
                {
                    return (true, deserializedBody);
                }
            }
            catch (JsonException e)
            {
                throw new Exception("Failed to parse server response.", e);
            }
            catch (HttpRequestException e)
            {
                // Log and handle HTTP request errors specifically
                throw new Exception($"Potentially network or server error when signing in: {e.Message}", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Error getting list of all products: {e.Message}", e);
            }
        }
    }
}
