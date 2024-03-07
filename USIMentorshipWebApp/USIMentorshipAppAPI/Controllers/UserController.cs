using MentorshipAppModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace MentorshipAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("GetUsers")]
        // IActionResult is our return type because it is flexible enough to return anything
        // We return an interface because what is returned (in this case it is anything)
        public IActionResult GetUsers()
        {
            List<User> users = new List<User>();

            User user = new User();
            user.UserId = "1";
            user.FirstName = "John";
            user.LastName = "Doe";
            user.Email = "johndoe@email.com";
            user.Industry = "IT";
            user.CurrentPosition = "Software Developer";
            user.Company = "Mead Johnson";
            user.Major = "Computer Science";
            user.Gender = "Male";
            user.AccountApprovalOption = "Approved";
            user.Role = "Mentor";
            users.Add(user);

            user = new User();
            user.UserId = "2";
            user.FirstName = "Jane";
            user.LastName = "Doe";
            user.Email = "johndoe@email.com";
            user.Industry = "IT";
            user.CurrentPosition = "Lawyer";
            user.Company = "Morgan and Morgan";
            user.Major = "Pre Law";
            user.Gender = "Female";
            user.AccountApprovalOption = "Approved";
            user.Role = "Mentor";
            users.Add(user);

            user = new User();
            user.UserId = "3";
            user.FirstName = "Joseph";
            user.LastName = "Doe";
            user.Email = "johndoe@email.com";
            user.Industry = "Healthcare";
            user.CurrentPosition = "Nurse";
            user.Company = "Deaconness";
            user.Major = "Nursing";
            user.Gender = "Male";
            user.AccountApprovalOption = "Approved";
            user.Role = "Mentor";
            users.Add(user);

            return Ok(users);
        }
    }
}
