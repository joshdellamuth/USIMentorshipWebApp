using Microsoft.EntityFrameworkCore;
using USIMentorshipWebApp.Models;

namespace USIMentorshipWebApp.Data
{
    public class RegistrationDataService
    {
        public State GetStateByCode(string code)
        {
            using UsiMentorshipApplicationContext registrationDataService = new UsiMentorshipApplicationContext();

            return registrationDataService.States.FirstOrDefault(s => s.StateCode == code);
        }

        public City GetCityById(int id)
        {
            using UsiMentorshipApplicationContext registrationDataService = new UsiMentorshipApplicationContext();

            return registrationDataService.Cities.FirstOrDefault(s => s.CityId == id);
        }

        public async Task<List<string>> GetIndustriesAsync()
        {
            using var context = new UsiMentorshipApplicationContext();
            return await context.Industries.Select(i => i.IndustryName).ToListAsync();
        }

        public async Task<List<string>> GetJobTitlesAsync()
        {
            using var context = new UsiMentorshipApplicationContext();
            return await context.JobTitles.Select(jt => jt.JobTitleName).ToListAsync();
        }

        public async Task<List<string>> GetSchoolsAsync()
        {
            using var context = new UsiMentorshipApplicationContext();
            return await context.Schools.Select(s => s.SchoolName).ToListAsync();
        }

    }
}
