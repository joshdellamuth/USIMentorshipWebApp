using MentorshipAppModels.Models;

namespace USIMentorshipWebApp.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<User>> GetUsers()
        {
            return await httpClient.GetFromJsonAsync<List<User>>("api/User/GetUsers");
        }
    }
}
