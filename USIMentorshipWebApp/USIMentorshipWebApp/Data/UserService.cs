using USIMentorshipWebApp.Models;
using System;
using System.Linq;
using BCrypt.Net;

namespace USIMentorshipWebApp.Data
{
    public class UserService
    {
        // add user must take in the name of a role also to make the new user. Every new user must have a role. 
        public void AddUser(User user, string roleName)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            // add the user and save the changes
            userContext.Add(user);
            userContext.SaveChanges();

            // query the roles table to return the item with the RoleName of "Mentor" or "Mentee"
            var mentorRole = userContext.Roles.FirstOrDefault(r => r.RoleName == roleName);

            if (mentorRole != null)
            {
                // grab the current date time so it can be added to the start date
                DateTime currentDateTime = DateTime.Now;

                // Create a new UserRole with the UserId of the added user and the RoleId of the "Mentor" role
                var userRole = new UserRole { UserId = user.UserId, RoleId = mentorRole.RoleId, StartDate = currentDateTime };

                // Add the UserRole to the UserRoles table and save changes
                userContext.UserRoles.Add(userRole);
                userContext.SaveChanges();
            }
        }

        public string HashPassword(string password)
        {
            // Generate a salt and hash the password
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }

        public void UpdateUser(User user)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();
            userContext.Update(user);
            userContext.SaveChanges();
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
