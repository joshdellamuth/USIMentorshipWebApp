using Microsoft.EntityFrameworkCore;
using USIMentorshipWebApp.Models;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace USIMentorshipWebApp.Data
{
    public class RegistrationDataService
    {
        private readonly HttpClient _http;

        public RegistrationDataService(HttpClient http)
        {
            _http = http;
        }

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


        public async Task<List<string>> GetCitiesByCountryName(string countryName)
        {
            var response = await _http.GetFromJsonAsync<List<City>>($"https://countriesnow.space/api/v0.1/countries/city?id={countryName}");

            return response.Select(c => c.CityName).ToList();
        }

        public async Task<List<string>> GetAllCountriesAsync()
        {
            var response = await _http.GetFromJsonAsync<CountryData>($"https://countriesnow.space/api/v0.1/countries");

            return response.data.Select(c => c.country).ToList();
        }

        public class University
        {
            public string name { get; set; }
            public List<string> domains { get; set; }
            public List<string> web_pages { get; set; }
            public string country { get; set; }
            public string alpha_two_code { get; set; }
            public string state_province { get; set; }
        }

        public async Task<List<string>> GetUniversityNamesAsync(string? countryName)
        {
            var httpClient = new HttpClient();
            // formats the countryName to go in the URL
            var formattedURLName = countryName.Replace(" ", "%20").ToLower();
            var response = await httpClient.GetStringAsync($"http://universities.hipolabs.com/search?country={formattedURLName}");
            var universities = JsonConvert.DeserializeObject<List<University>>(response);
            var universityNames = new HashSet<string>();

            foreach (var university in universities)
            {
                universityNames.Add(university.name);
            }

            return universityNames.ToList();
        }

        // get University countries
        public async Task<List<string>> GetUniversityCountryNamesAsync()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync("http://universities.hipolabs.com/search");
            var universities = JsonConvert.DeserializeObject<List<University>>(response);
            var countryNames = new HashSet<string>();

            foreach (var university in universities)
            {
                countryNames.Add(university.country);
            }

            return countryNames.ToList();
        }

        // get state by country if it has any (null if not)



        // get city by state 

        //get city by country if the city does not have a state 

        public class CountryTest
        {
            public string iso2 { get; set; }
            public string iso3 { get; set; }
            public string country { get; set; }
            public List<string> cities { get; set; }
        }

        public class CountryData
        {
            public bool error { get; set; }
            public string msg { get; set; }
            public List<CountryTest> data { get; set; }
        }
    }
}
