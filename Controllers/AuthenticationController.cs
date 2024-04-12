using Microsoft.AspNetCore.Mvc;
using noeTaskManager_app.Models.AuthModels;

namespace noeTaskManager_app.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

       /* //Actions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthSignIn()
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
        } */

        //Views 
        public IActionResult Signin()
        {
         

            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }
    }
}
