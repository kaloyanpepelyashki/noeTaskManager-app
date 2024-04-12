namespace noeTaskManager_app.Models.AuthModels
{
    public class SigninResponseObject
    {
        public string AccessToken { get; set; }
        public UserCredsResponse UserCreds { get; set; }
    }
}
