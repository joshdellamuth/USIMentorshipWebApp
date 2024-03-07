using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorshipAppModels.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string Industry { get; set; }
        public string CurrentPosition { get; set; }
        public string Company { get; set; }
        public string Major { get; set; }
        public string? Minor { get; set; }
        public string? Bio { get; set; }

        public string? LinkedIn { get; set; }
        public string? BusinessCountry { get; set; }
        public string? BusinessState { get; set; }
        public string BusinessCity { get; set; }
        public string? ClassYear { get; set; }
        public string Gender { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string AccountApprovalOption { get; set; }

        public string Role { get; set; }
        public string[,] DisplayRequests = new string[10, 10];
    }
}
