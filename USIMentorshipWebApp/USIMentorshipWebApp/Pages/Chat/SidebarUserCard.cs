using USIMentorshipWebApp.Models;

namespace USIMentorshipWebApp.Pages.Chat
{
    public class SidebarUserCard
    {
        public List<User> UsersInMatch { get; set; }
        public MatchCommunicationDetail? MostRecentChat { get; set; }
    }
}
