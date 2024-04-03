using Microsoft.EntityFrameworkCore;
using USIMentorshipWebApp.Models;

namespace USIMentorshipWebApp.Data
{
    public class RequestService
    {
        // add user must take in the name of a role also to make the new user. Every new user must have a role. 
        public void RequestOnePersonMatch(User sender, User reciever, int mentorshipLengthInSemesters)
        {
            using UsiMentorshipApplicationContext requestService = new UsiMentorshipApplicationContext();

            // A mentorship if 4 months, so we will set the EndDate the currentdate and add the monthsOfMatchRequest months 
            int monthsOfMatchRequest = mentorshipLengthInSemesters * 4;
            Match match = new Match { Status = "Pending", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(monthsOfMatchRequest) };

            // add the match and to the Matches table
            requestService.Add(match);

            // Need to save the changes so the next queries can be ran on match
            requestService.SaveChanges();


            // add entry in UserMatches table to link these two users to our match we just made 
            UserMatch senderUserMatch = new UserMatch { UserId = sender.UserId, MatchId = match.MatchId };
            UserMatch recieverUserMatch = new UserMatch { UserId = reciever.UserId, MatchId = match.MatchId };

            // add both of the UserMatches we jsut made
            requestService.Add(senderUserMatch);
            requestService.Add(recieverUserMatch);

            requestService.SaveChanges();
        }
    }
}
