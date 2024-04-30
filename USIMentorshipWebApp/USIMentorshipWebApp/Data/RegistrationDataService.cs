using Microsoft.EntityFrameworkCore;
using USIMentorshipWebApp.Models;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace USIMentorshipWebApp.Data
{
    public class RegistrationDataService
    {
        private readonly HttpClient _http;

        public RegistrationDataService(HttpClient http)
        {
            _http = http;
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
            // ACCOUNT FOR NULL VALES
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
        public async Task<List<string>>? GetStatesByCountryAsync(string? countryName)
        {
            var httpClient = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(new { country = countryName }), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://countriesnow.space/api/v0.1/countries/states", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var countryResponse = JsonConvert.DeserializeObject<CountryResponse>(jsonResponse);

                if (!countryResponse.error)
                {
                    // Extract state names from the list of State objects
                    return countryResponse.data.states.Select(state => state.name).ToList();
                }
            }

            return null;
        }


        // get city by state and country
        public async Task<List<string>>? GetCityByStateAndCountryAsync(string? countryName, string? stateName)
        {
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(new { country = countryName, state = stateName }), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://countriesnow.space/api/v0.1/countries/state/cities", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var citiesResponse = JsonConvert.DeserializeObject<CitiesResponse>(result);

                if (citiesResponse != null && !citiesResponse.error && citiesResponse.data != null)
                {
                    return new List<string>(citiesResponse.data);
                }
            }

            return null;
        }

        // get city by country
        public async Task<List<string>>? GetCityByCountryAsync(string? countryName)
        {
            var httpClient = new HttpClient();
            // (new { country = countryName }) make an anonomyous object with a property called country and sets it equal to the countryName
            // application/json is the MIME type (can look that up
            var content = new StringContent(JsonConvert.SerializeObject(new { country = countryName }), Encoding.UTF8, "application/json");

            //does a post method at the specified URL and passes in content
            var response = await httpClient.PostAsync("https://countriesnow.space/api/v0.1/countries/cities", content);

            // looks at our code we get back
            if (response.IsSuccessStatusCode)
            {
                // looks at the content of the response
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // converts the json response into Country response 
                var citiesResponse = JsonConvert.DeserializeObject<CitiesResponse>(jsonResponse);

                if (!citiesResponse.error)
                {
                    return citiesResponse.data;
                }
            }

            return null;
        }


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
            public List<CountryTest>? data { get; set; }
            public List<State>? states { get; set; }
        }

        public class State
        {
            public string name { get; set; }
            public string state_code { get; set; }
        }

        public class CountryResponse
        {
            public bool error { get; set; }
            public string msg { get; set; }
            public CountryData data { get; set; }
        }

        public class CitiesResponse
        {
            public bool error { get; set; }
            public string msg { get; set; }
            public List<string> data { get; set; }
        }
    }
}
