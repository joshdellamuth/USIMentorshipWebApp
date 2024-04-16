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

            // set the match status to pending and startdate to now
            Match match = new Match { Status = "Pending", StartDate = DateTime.Now };

            // add the match and to the Matches table
            requestService.Add(match);

            // Need to save the changes so the next queries can be ran on match
            requestService.SaveChanges();

            

            // add entry in UserMatches table to link these two users to our match we just made 
            UserMatch senderUserMatch = new UserMatch { UserId = sender.UserId, MatchId = match.MatchId };
            UserMatch recieverUserMatch = new UserMatch { UserId = reciever.UserId, MatchId = match.MatchId };

            // add both of the UserMatches we just made
            requestService.Add(senderUserMatch);
            requestService.Add(recieverUserMatch);

            requestService.SaveChanges();

            // send an email to the person who was sent the request 
            EmailService emailService = new EmailService();
            string emailSubject = $"New Mentorship Request from {sender.FirstName} {sender.LastName}!";
            string emailBody = $"Hi {reciever.FirstName}! <br/><br/>{sender.FirstName} {sender.LastName} has sent you a mentorship request on your USI Mentorship account. You can accept or dismiss the request by logging on to the USI Mentorship Portal and navigating to the 'Requests' page. <br/><br/> Login Here:<br/>www.google.com";

            // send email to person getting the request 
            await emailService.SendMail(reciever.EmailAddress, emailSubject, emailBody);
        }
    }
}
