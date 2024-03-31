using USIMentorshipWebApp.Models;
using System;
using System.Linq;
using BCrypt.Net;

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

        public User GetUserByEmail(string emailAddress)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();
            // Retrieve the user by email address
            return userContext.Users.FirstOrDefault(u => u.EmailAddress == emailAddress);
        }

        public bool VerifyPassword(User user, string password)
        {
            // Verify the password using Bcrypt
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
    }
}
