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

        //Actions
        public async Task<ActionResult> ActionCreateTask(CreateTask newTask)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateTask", newTask);
            }
            try
            {
                var response = await _taskManagerService.InsertATask(newTask);

                if (response)
                {
                    TempData["success"] = "Task was created successfuly";
                    
                    return RedirectToAction("Index");

                } else
                {
                    //In case the API didn't respond with a success status code
                    ModelState.AddModelError("", "Failed to create a task");
                }
            } catch(Exception e) 
            {
                _logger.LogError(e, "Error creating task");
                ModelState.AddModelError("", "Error creating a task");
            }

            return View("CreateTask", newTask);
        }


        //Viewes
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

        public IActionResult CreateTask()
        {
            TempData["Success"] = null;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
