using Microsoft.AspNetCore.Mvc;
using noeTaskManager_app.Models;
using noeTaskManager_app.Services.Interfaces;
using System.Diagnostics;

namespace noeTaskManager_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ITaskManagerService _taskManagerService;

        public HomeController(ILogger<HomeController> logger, ITaskManagerService taskManagerService)
        {
            _logger = logger;
            _taskManagerService = taskManagerService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                (bool isSuccess, List<TaskItem>? tasksList) tasks = await _taskManagerService.GetAllTasks();

                if (tasks.isSuccess)
                {
                    ViewData["tasks"] = tasks.tasksList;
                    return View();
                }
                else
                {
                    ViewData["tasks"] = null;
                    return View();
                }
            } catch (Exception e)
            {
                _logger.LogError(e, "Fetching tasks");
                ViewData["ErrorMessage"] = "An error occurred: " + e.Message;
                return View();
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
