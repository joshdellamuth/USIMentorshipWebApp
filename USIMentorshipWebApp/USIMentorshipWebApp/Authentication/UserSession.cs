namespace BlazorServerAuthenticationAndAuthorization.Authentication
{
    // provide more session properties here as needed
    public class UserSession
    {
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}