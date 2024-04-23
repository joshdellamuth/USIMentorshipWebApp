using Microsoft.EntityFrameworkCore;
using USIMentorshipWebApp.Models;

namespace USIMentorshipWebApp.Data
{
    public class RequestService
    {
        // add user must take in the name of a role also to make the new user. Every new user must have a role. 
        public async Task RequestOnePersonMatch(User sender, User reciever)
        {
            using UsiMentorshipApplicationContext requestService = new UsiMentorshipApplicationContext();

            // Check if a match between the two users already exists
            int? existingMatch = requestService.UserMatches
                .Where(um => um.UserId == sender.UserId || um.UserId == reciever.UserId)
                .GroupBy(um => um.MatchId)
                .Where(g => g.Count() == 2)
                .Select(g => g.Key)
                .FirstOrDefault();

            Match? match;
            match = requestService.Matches.Find(existingMatch);

            if (match != null)
            {
                // If the match exists, set the Status to "Pending", the StartDate to the current DateTime and the EndDate to NULL
                match.Status = "Pending";
                match.StartDate = DateTime.Now;
                match.EndDate = null;
            }
            else
            {
                // If the match does not exist, create a new one
                match = new Match { Status = "Pending", StartDate = DateTime.Now };
                requestService.Add(match);
                requestService.SaveChanges();

                // Add entry in UserMatches table to link these two users to our match we just made 
                UserMatch senderUserMatch = new UserMatch { UserId = sender.UserId, MatchId = match.MatchId };
                UserMatch recieverUserMatch = new UserMatch { UserId = reciever.UserId, MatchId = match.MatchId };

                // Add both of the UserMatches we just made
                requestService.Add(senderUserMatch);
                requestService.Add(recieverUserMatch);
            }

            requestService.SaveChanges();

            // Send an email to the person who was sent the request 
            EmailService emailService = new EmailService();
            string emailSubject = $"New Mentorship Request from {sender.FirstName} {sender.LastName}!";
            string emailBody = $"Hi {reciever.FirstName}! <br/><br/>{sender.FirstName} {sender.LastName} has sent you a mentorship request on your USI Mentorship account. You can accept or dismiss the request by logging on to the USI Mentorship Portal and navigating to the 'Requests' page. <br/><br/> Login Here:<br/>www.google.com";
        }
    }
}
