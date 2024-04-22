using Microsoft.AspNetCore.Mvc;
using noeTaskManager_app.Models.AuthModels;
using noeTaskManager_app.Services;
using noeTaskManager_app.Services.Interfaces;

namespace noeTaskManager_app.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private IAuthService _authService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthService AuthService)
        {
            _logger = logger;
            _authService = AuthService;
        }

       //Actions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AuthSignIn(SigninModel signinCreds)
        {
            if (!ModelState.IsValid)
            {
                return View("Signin", signinCreds);
            }
            try
            {
                var response = await _authService.SignIn(signinCreds.Email, signinCreds.Password);
                var tokenIsValid = String.IsNullOrWhiteSpace(_authService.GetTokenCookie());

                Console.WriteLine($"token : {_authService.GetTokenCookie()}");
        
                //Checks if there was a response back and if a token was registered
                if (response.IsSuccess && response.signinResponseObject != null && !tokenIsValid)
                {
                    
                    return RedirectToAction("Index", "Home");
               
                }
                else
                {
                    // In case the auth process was invalid and no response was returned neither a token registered, pushes a model error
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during sign-in.");
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
            }



            return View("Signin", signinCreds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AuthSignUp(SignupModel signupCreds)
        {
            if (!ModelState.IsValid)
            {
                return View("Signup", signupCreds);
            }
            try
            {
                var response = await _authService.SignUp(signupCreds.FirstName, signupCreds.LastName, signupCreds.Email, signupCreds.Password);
                if(response.IsSuccess && response.userCredsResponse != null)
                {
                    return View("Signin");
                } else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during sign-in.");
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
            }


            return View("Signup", signupCreds);
        }


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
