using Microsoft.EntityFrameworkCore;
using USIMentorshipWebApp.Models;

namespace USIMentorshipWebApp.Data
{
    public class UserService
    {
        // Get User By UserId
        public User GetUserByIdAsync(int userId)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            try
            {
                return userContext.Users
                    .FirstOrDefault(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., logging, error handling)
                throw new ApplicationException("Error retrieving user by ID.", ex);
            }
        }

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

        public User? GetUserByEmailAndPassword(string emailAddress, string password)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            // Retrieve the user by email address
            User? user = userContext.Users.FirstOrDefault(u => u.EmailAddress == emailAddress);

            // If the user exists and the password is correct, return the user
            if (user != null && user.Password == password)
            {
                return user;
            }
            // If the user doesn't exist or the password is incorrect, return null
            else
            {
                return null;
            }
        }

        public User? GetExampleMenteeUser()
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            // gets the role of Mentee
            Role? menteeRole = userContext.Roles.FirstOrDefault(r => r.RoleName == "Mentee");


            User? userWithMenteeRole = (from user in userContext.Users
                                        join userRole in userContext.UserRoles on user.UserId equals userRole.UserId
                                        where userRole.RoleId == menteeRole.RoleId
                                        select user).FirstOrDefault();

            return userWithMenteeRole;
        }

        public User? GetExampleMentorUser()
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            // gets the role of Mentee
            Role? mentorRole = userContext.Roles.FirstOrDefault(r => r.RoleName == "Mentor");

            User? userWithMenteeRole = (from user in userContext.Users
                                        join userRole in userContext.UserRoles on user.UserId equals userRole.UserId
                                        where userRole.RoleId == mentorRole.RoleId
                                        select user).FirstOrDefault();

            return userWithMenteeRole;
        }

        public User? GetExampleAdminUser()
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            // gets the role of Mentee
            Role? adminRole = userContext.Roles.FirstOrDefault(r => r.RoleName == "Admin");


            User? userWithAdminRole = (from user in userContext.Users
                                        join userRole in userContext.UserRoles on user.UserId equals userRole.UserId
                                        where userRole.RoleId == adminRole.RoleId
                                        select user).FirstOrDefault();

            return userWithAdminRole;
        }

        //public Role? GetUserRoleByUserId(string userId)
        //{
        //    using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

        //    // Retrieve the user by email address
        //    Role? role = userContext.Users.FirstOrDefault(u => u.EmailAddress == emailAddress);
        //    return userContext.Users
        //        .Join(
        //            userContext.UserRoles,
        //            user => user.UserId,
        //            userRole => userRole.UserId,
        //            (user, userRole) => new { User = user, UserRole = userRole }
        //                )
        //        .Join(
        //            userContext.Roles,
        //            combined => combined.UserRole.RoleId,
        //            role => role.RoleId,
        //            (combined, role) => new { User = combined.User, Role = role }
        //        )
        //        .Where(combined => combined.User.UserId == userId)
        //        .Select(combined => combined.Role)
        //        .FirstOrDefault();
        //}
    }
}
