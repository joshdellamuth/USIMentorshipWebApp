using USIMentorshipWebApp.Models;

namespace USIMentorshipWebApp.Data
{
    public class UserService
    {
        public void AddUser(User user)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();
            userContext.Add(user);
            userContext.SaveChanges();
        }
    }
}
