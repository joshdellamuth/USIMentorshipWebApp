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

        public string HashPassword(string password)
        {
            // Generate a salt and hash the password
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }
    }
}
