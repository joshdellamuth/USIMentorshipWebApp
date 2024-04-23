using Microsoft.EntityFrameworkCore;
using USIMentorshipWebApp.Models;
using USIMentorshipWebApp.Pages.Chat;
using Match = USIMentorshipWebApp.Models.Match;

namespace USIMentorshipWebApp.Data
{
    public class AdminPortalService
    {
        public async Task<List<User>?> GetAllUsers()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allUsers = await adminPortalService.Users.ToListAsync();

            return allUsers;
        }

        public async Task<List<UserDetail>?> GetAllUserDetails()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allUserDetails = await adminPortalService.UserDetails.ToListAsync();

            return allUserDetails;
        }

        public async Task<List<UserMatch>?> GetAllUserMatches()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allUserMatches = await adminPortalService.UserMatches.ToListAsync();

            return allUserMatches;
        }

        public async Task<List<Match>?> GetAllMatches()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allMatches = await adminPortalService.Matches.ToListAsync();

            return allMatches;
        }

        public async Task<List<UserRole>?> GetAllUserRoles()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allUserRoles = await adminPortalService.UserRoles.ToListAsync();

            return allUserRoles;
        }

        public async Task<List<Role>?> GetAllRoles()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allRoles = await adminPortalService.Roles.ToListAsync();

            return allRoles;
        }

        public async Task<List<MatchCommunicationDetail>?> GetAllMatchCommunicationDetails()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allMatchCommunicationDetails = await adminPortalService.MatchCommunicationDetails.ToListAsync();

            return allMatchCommunicationDetails;
        }

        public async Task<List<ConditionDetail>?> GetAllConditionDetails()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allConditionDetails = await adminPortalService.ConditionDetails.ToListAsync();

            return allConditionDetails;
        }

        public async Task<List<Industry>?> GetAllIndustries()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allIndustries = await adminPortalService.Industries.ToListAsync();

            return allIndustries;
        }

        public async Task<List<JobTitle>?> GetAllJobTitles()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allJobTitles = await adminPortalService.JobTitles.ToListAsync();

            return allJobTitles;
        }

        public async Task<List<School>?> GetAllSchools()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allSchools = await adminPortalService.Schools.ToListAsync();

            return allSchools;
        }

        public async Task<List<Country>?> GetAllCountries()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allCountries = await adminPortalService.Countries.ToListAsync();

            return allCountries;
        }

        public async Task<List<State>?> GetAllStates()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allStates = await adminPortalService.States.ToListAsync();

            return allStates;
        }

        public async Task<List<City>?> GetAllCities()
        {
            using UsiMentorshipApplicationContext adminPortalService = new UsiMentorshipApplicationContext();

            var allCities = await adminPortalService.Cities.ToListAsync();

            return allCities;
        }
    }
}
