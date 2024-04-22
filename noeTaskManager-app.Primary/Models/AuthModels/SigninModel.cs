namespace noeTaskManager_app.Models.AuthModels
{
    //This model handles the data comming from the sign-in form
    public class SigninModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
