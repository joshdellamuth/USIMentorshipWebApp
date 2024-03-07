using MentorshipAppModels.Models;

namespace USIMentorshipWebApp.Services
{
    public interface IUserService
    {
        Task<List<User>> GetUsers();
    }
}
