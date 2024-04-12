using noeTaskManager_app.Models.AuthModels;

namespace noeTaskManager_app.Services.Interfaces
{
    public interface IAuthService
    {
        Task<SigninResponseObject> SignIn(string email, string password);
        Task<bool> SignUp(string firstName, string lastName, string email, string password);
    }
}
