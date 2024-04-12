namespace noeTaskManager_app.Models.AuthModels
{
    //This model handles the data comming from the sign-in form
    public class SigninModel(string email, string password)
    {
        string Email { get; set; } = email;
        string Password { get; set; } = password;
    }
}
