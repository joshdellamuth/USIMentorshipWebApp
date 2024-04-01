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

        public List<string> GetJobTitles()
        {
            using UsiMentorshipApplicationContext registrationDataService = new UsiMentorshipApplicationContext();

            // jt represents a single JobTitle in the JobTitles DbSet
            return registrationDataService.JobTitles.Select(jt => jt.JobTitleName).ToList();
        }

        public List<string> GetSchools()
        {
            using UsiMentorshipApplicationContext registrationDataService = new UsiMentorshipApplicationContext();

            return registrationDataService.Schools.Select(s => s.SchoolName).ToList();
        }

        public List<string> GetIndustries()
        {
            using UsiMentorshipApplicationContext registrationDataService = new UsiMentorshipApplicationContext();

            return registrationDataService.Industries.Select(i => i.IndustryName).ToList();
        }
    }
}
