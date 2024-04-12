namespace noeTaskManager_app.Models.AuthModels
{
    public class SignupModel(string firstName, string lastName, string email, string password)
    {
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string Email { get; set; } = email;
        public string Password { get; set; } = password;
    }
}
