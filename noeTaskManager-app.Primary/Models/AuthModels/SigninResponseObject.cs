using System.Text.Json.Serialization;

namespace noeTaskManager_app.Models.AuthModels
{
    public class SigninResponseObject
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("user_cred")]
        public UserCredsResponse UserCreds { get; set; }
    }
}
