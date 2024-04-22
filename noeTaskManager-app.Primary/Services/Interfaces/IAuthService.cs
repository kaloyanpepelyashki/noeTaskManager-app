using noeTaskManager_app.Models.AuthModels;

namespace noeTaskManager_app.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool IsSuccess, SigninResponseObject signinResponseObject)> SignIn(string email, string password);
        Task<(bool IsSuccess, UserCredsResponse userCredsResponse)> SignUp(string firstName, string lastName, string email, string password);
        string GetTokenCookie();
    }
}
