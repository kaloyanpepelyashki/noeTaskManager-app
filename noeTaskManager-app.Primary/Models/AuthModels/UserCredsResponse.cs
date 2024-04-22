using System.Text.Json.Serialization;

namespace noeTaskManager_app.Models.AuthModels
{
    public class UserCredsResponse
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("userUID")]

        public string UserUId { get; set; }
    }
}
