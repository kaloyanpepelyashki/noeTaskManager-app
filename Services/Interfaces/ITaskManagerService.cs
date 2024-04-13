using noeTaskManager_app.Models;

namespace noeTaskManager_app.Services.Interfaces
{
    public interface ITaskManagerService
    {
        Task<List<TaskItem>> GetAllTasks();
    }
}
