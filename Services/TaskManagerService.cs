using noeTaskManager_app.Models;
using noeTaskManager_app.Services.Interfaces;
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

        public async Task<List<TaskItem>> GetAllTasks()
        {
            try
            {

            } catch (Exception ex) { 
            }
        }
    }
}
