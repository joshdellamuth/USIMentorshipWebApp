using USIMentorshipWebApp.Models;

namespace USIMentorshipWebApp.Pages.Requests
{
    public class MatchRequestDisplayCardObject
    {
        public Match? CorrespondingMatch { get; set; }
        public User? RequestedUser { get; set; }
    }
}
