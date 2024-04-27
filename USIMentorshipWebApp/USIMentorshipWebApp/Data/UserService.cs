using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using USIMentorshipWebApp.Models;

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

        //public async Task<List<User?>> GetAllMentors()
        //{
        //    using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

        //    var allMentors = await userContext.Users
        //        .Join(
        //            userContext.UserRoles,
        //            user => user.UserId,
        //            userRole => userRole.UserId,
        //            (user, userRole) => new { User = user, UserRole = userRole }
        //            )
        //        .Join(
        //            userContext.Roles,
        //            combined => combined.UserRole.RoleId,
        //            role => role.RoleId,
        //            (combined, role) => new { User = combined.User, Role = role }
        //            )
        //        .Where(combined => combined.Role.RoleName == "Mentor")
        //        .Select(combined => combined.User)
        //        .ToListAsync();

        //    return allMentors;
        //}

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

        public async Task<Country?> GetCountryByIdAsync(string countryCode)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            Country? country = userContext.Countries
                    .Where(c => c.CountryCode == countryCode)
                    .FirstOrDefault();

            return country;
        }

        public async Task<State?> GetStateByIdAsync(string stateCode)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            State? country = userContext.States
                    .Where(c => c.StateCode == stateCode)
                    .FirstOrDefault();

            return country;
        }

        public async Task<City?> GetCityByIdAsync(int? cityCode)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            City? country = userContext.Cities
                    .Where(c => c.CityId == cityCode)
                    .FirstOrDefault();

            return country;
        }

        public async Task<List<User>> GetUsers()
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();
            List<User> userList = new List<User>();
            userList = userContext.Users.ToList();
            return userList;
        }

        #region Mentor Match Methods to retrieve the data
        public async Task<List<City>> GetMentorCitiesGivenAllMentorsAsync(List<User?> allMentors)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            var cities = allMentors
                .Select(user => user.BusinessCityId) // Select BusinessCityId from each user
                .Distinct() // Remove duplicates
                .Join(userContext.Cities, // Join with Cities table
                    businessCityId => businessCityId, // specifying our list of businessCityIds as businessCityId
                    city => city.CityId, // Key from the Cities table
                    (businessCityId, city) => city) // Select CityName
                .ToList(); // Convert to List

            return cities;
        }
        #endregion

        #region Mentor Match Methods to retrieve the data
        public async Task<List<State>> GetMentorStatesGivenAllMentorsAsync(List<User?> allMentors)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            var states = allMentors
                .Select(user => user.BusinessStateCode) // Select BusinessCityId from each user
                .Distinct() // Remove duplicates
                .Join(userContext.States, // Join with Cities table
                    businessStateCode => businessStateCode, // specifying our list of businessCityIds as businessCityId
                    state => state.StateCode, // Key from the Cities table
                    (businessCityId, state) => state) // Select CityName
                .ToList(); // Convert to List

            return states;
        }

        public async Task<List<Country>> GetMentorCountriesGivenAllMentorsAsync(List<User?> allMentors)
        {
            using UsiMentorshipApplicationContext userContext = new UsiMentorshipApplicationContext();

            var countries = allMentors
                .Select(user => user.BusinessCountryCode) // Select BusinessCityId from each user
                .Distinct() // Remove duplicates
                .Join(userContext.Countries, // Join with Cities table
                    businessCountryCode => businessCountryCode, // specifying our list of businessCityIds as businessCityId
                    country => country.CountryCode, // Key from the Cities table
                    (businessCountryId, country) => country) // Select CityName
                .ToList(); // Convert to List

            return countries;
        }
        #endregion
    }
}
