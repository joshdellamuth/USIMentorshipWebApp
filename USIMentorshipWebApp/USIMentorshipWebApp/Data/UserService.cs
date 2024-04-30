using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using USIMentorshipWebApp.Models;
using Microsoft.Extensions.Caching.Memory;
using System;


namespace USIMentorshipWebApp.Data
{
    public class UserService
    {
        // Get User By UserId
        // Get User By UserId

        public async Task<User>? GetUserByIdAsync(int? userId)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            try
            {
                return await userContext.Users
                    .FirstOrDefaultAsync(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // add user must take in the name of a role also to make the new user. Every new user must have a role. 
        public async Task AddUserAsync(User user, string roleName)
        {
            using var userContext = new UsiMentorshipApplicationContext();

            // Add the user asynchronously and save the changes
            await userContext.AddAsync(user);
            await userContext.SaveChangesAsync();

            // Query the roles table asynchronously to return the item with the RoleName of "Mentor" or "Mentee"
            var mentorRole = await userContext.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);

            if (mentorRole != null)
            {
                // Grab the current date time so it can be added to the start date
                DateTime currentDateTime = DateTime.Now;

                // Create a new UserRole with the UserId of the added user and the RoleId of the "Mentor" role
                var userRole = new UserRole { UserId = user.UserId, RoleId = mentorRole.RoleId, StartDate = currentDateTime };

                // Add the UserRole to the UserRoles table and save changes asynchronously
                userContext.UserRoles.Add(userRole);
                await userContext.SaveChangesAsync();
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
            //return BCrypt.Net.BCrypt.Verify(password, user.Password);
            UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();
            return userContext.Users.Any(u => u.UserId == user.UserId && u.Password == password);

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



        public async Task<List<User>> GetAllMentors()
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            var usersWithMentorRole = from user in userContext.Users
                                      join userRole in userContext.UserRoles on user.UserId equals userRole.UserId
                                      join role in userContext.Roles on userRole.RoleId equals role.RoleId
                                      where role.RoleName == "Mentor"
                                      select user;

            return await usersWithMentorRole.ToListAsync();
        }


        // MENTOR INDUSTRIES 
        #region All the commands to get the mentor information
        public async Task<List<string?>> GetMentorIndustries()
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            var allMentors = await GetAllMentors(); 

            var industries = allMentors.Where(u => !string.IsNullOrEmpty(u.Industry)).Select(u => u.Industry).Distinct().ToList();

            return industries; 
        }

        // MENTOR CURRENT POSITIONS 
        public async Task<List<string?>> GetMentorCurrentPositions()
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            var allMentors = await GetAllMentors();

            var currentPositions = allMentors.Where(u => !string.IsNullOrEmpty(u.CurrentPosition)).Select(u => u.CurrentPosition).Distinct().ToList();

            return currentPositions;
        }

        // MENTOR MAJORS
        public async Task<List<string?>> GetMentorMajors()
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            var allMentors = await GetAllMentors();

            var majors = allMentors.Where(u => !string.IsNullOrEmpty(u.Major)).Select(u => u.Major).Distinct().ToList();

            return majors;
        }

        // MENTOR COMPANIES
        public async Task<List<string?>> GetMentorCompanies()
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            var allMentors = await GetAllMentors();

            var companies = allMentors.Where(u => !string.IsNullOrEmpty(u.Company)).Select(u => u.Company).Distinct().ToList();

            return companies;
        }

        #endregion


        public async Task<User> GetExampleMenteeUser()
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

        public async Task<User?> GetExampleMentorUser()
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

        public async Task<User?> GetExampleAdminUser()
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

        public async Task<List<User>> GetUsers()
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();
            List<User> userList = new List<User>();
            userList = userContext.Users.ToList();
            return userList;
        }

        #region Mentor Match Methods to retrieve the data
        public async Task<List<string>> GetMentorCitiesGivenAllMentorsAsync(List<User?> allMentors)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            var cities = allMentors
                .Where(u => u?.BusinessCity != null) // Exclude null entries
                .Select(u => u.BusinessCity)
                .Distinct()
                .ToList();

            return cities;
        }

        public async Task<List<string>> GetMentorStatesGivenAllMentorsAsync(List<User?> allMentors)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            var states = allMentors
                .Where(u => u?.BusinessState != null) // Exclude null entries
                .Select(u => u.BusinessState)
                .Distinct()
                .ToList();

            return states;
        }

        public async Task<List<string>> GetMentorCountriesGivenAllMentorsAsync(List<User?> allMentors)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            var countries = allMentors
                .Where(u => u?.BusinessCountry != null) // Exclude null entries
                .Select(u => u.BusinessCountry)
                .Distinct()
                .ToList();

            return countries;
        }
        #endregion
    }
}
