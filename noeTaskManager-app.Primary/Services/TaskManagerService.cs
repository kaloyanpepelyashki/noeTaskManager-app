using noeTaskManager_app.Models;
using noeTaskManager_app.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace noeTaskManager_app.Services
{
    public class TaskManagerService: ITaskManagerService
    {
        protected string _serverUrl;
        protected HttpClient _httpClient;
        protected IHttpContextAccessor _httpContextAccessor;
        protected IAuthService _authService;

        public TaskManagerService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IAuthService authenticationService)
        {
            _serverUrl = "http://localhost:5241/api/";

            _authService = authenticationService;

            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authService.GetTokenCookie());
        }

        public async Task<(bool isSuccess, List<TaskItem>?)> GetAllTasks()
        {
            try
            {
                var endpoint = $"{_serverUrl}GetTask/all";

                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };
                List<TaskItem>? deserializedBody = JsonSerializer.Deserialize<List<TaskItem>>(responseBody, serializerOptions);

                if (deserializedBody.Count == 0)
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
                throw new Exception($"Potentially network or server error when retreiving tasks: {e.Message}", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Error getting list of all products: {e.Message}", e);
            }
        }

        public async Task<bool> InsertATask(CreateTask newTask)
        {
            try
            {
                var endpoint = $"{_serverUrl}CreateTask";
                var taskProperties = new
                {
                    summary = newTask.Summary,
                    description = newTask.Description,
                    priority = newTask.Priority,
                    dueDate = newTask.DueDate,
                };

                var serializedObject = JsonSerializer.Serialize(taskProperties);
                var data = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, data);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                if(responseBody == "Task created")
                {
                    return true;
                } else
                {
                    return false;
                }

            }
            catch (JsonException e)
            {
                throw new Exception("Failed to parse server response.", e);
            }
            catch (HttpRequestException e)
            {
                // Log and handle HTTP request errors specifically
                throw new Exception($"Potentially network or server error when creating a task: {e.Message}", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Error creating a new task: {e.Message}", e);
            }
        }

        public async Task<bool> DeleteATask(string targetTaskKey)
        {
            try
            {
                var endpoint = $"{_serverUrl}DeleteTask/deleteByKey/{targetTaskKey}";

                var response = await _httpClient.DeleteAsync(endpoint);

                var responseBody = await response.Content.ReadAsStringAsync();
                if(responseBody == "Task was deleted")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (HttpRequestException e)
            {
                // Log and handle HTTP request errors specifically
                throw new Exception($"Potentially network or server error when creating a task: {e.Message}", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Error creating a new task: {e.Message}", e);
            }
        }
    }
}
