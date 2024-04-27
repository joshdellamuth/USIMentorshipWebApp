using Azure;
using Microsoft.EntityFrameworkCore;
using USIMentorshipWebApp.Models;
using USIMentorshipWebApp.Pages.Chat;
using USIMentorshipWebApp.Pages.Requests;

namespace USIMentorshipWebApp.Data
{
    public class RequestService
    {
        // add user must take in the name of a role also to make the new user. Every new user must have a role. 
        public async Task RequestOnePersonMatch(User sender, User reciever)
        {
            using UsiMentorshipApplicationContext requestContext = new UsiMentorshipApplicationContext();

            // Check if a match between the two users already exists
            int? existingMatch = requestContext.UserMatches
                .Where(um => um.UserId == sender.UserId || um.UserId == reciever.UserId)
                .GroupBy(um => um.MatchId)
                .Where(g => g.Count() == 2)
                .Select(g => g.Key)
                .FirstOrDefault();

            Match? match;
            match = requestContext.Matches.Find(existingMatch);

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
                requestContext.Add(match);
                requestContext.SaveChanges();

                // Add entry in UserMatches table to link these two users to our match we just made 
                UserMatch senderUserMatch = new UserMatch { UserId = sender.UserId, MatchId = match.MatchId };
                UserMatch recieverUserMatch = new UserMatch { UserId = reciever.UserId, MatchId = match.MatchId };

                // Add both of the UserMatches we just made
                requestContext.Add(senderUserMatch);
                requestContext.Add(recieverUserMatch);
            }

            requestContext.SaveChanges();

            // Send an email to the person who was sent the request 
            EmailService emailService = new EmailService();
            string emailSubject = $"New Mentorship Request from {sender.FirstName} {sender.LastName}!";
            string emailBody = $"Hi {reciever.FirstName}! <br/><br/>{sender.FirstName} {sender.LastName} has sent you a mentorship request on your USI Mentorship account. You can accept or dismiss the request by logging on to the USI Mentorship Portal and navigating to the 'Requests' page. <br/><br/> Login Here:<br/>www.google.com";
        }

        public async Task<Match>? GetMatchByMatchId(int? matchId)
        {
            using (var context = new UsiMentorshipApplicationContext())
            {
                var match = await context.Matches
                        .Where(m => m.MatchId == matchId)
                        .FirstAsync();

                return match;
            }
        }

        public async Task<List<Match>>? GetPendingMatchesByUserId(int? userId)
        {
            using (var context = new UsiMentorshipApplicationContext())
            {
                var matches = await context.UserMatches
                    .Where(um => um.UserId == userId)
                    .Join(context.Matches,
                        um => um.MatchId,
                        m => m.MatchId,
                        (um, m) => m)
                    .Where(m => m.Status == "Pending")
                    .ToListAsync();

                return matches;
            }
        }

        public async Task<List<Match>>? GetApprovedMatchesByUserId(int? userId)
        {
            using (var context = new UsiMentorshipApplicationContext())
            {
                var matches = await context.UserMatches
                    .Where(um => um.UserId == userId)
                    .Join(context.Matches,
                        um => um.MatchId,
                        m => m.MatchId,
                        (um, m) => m)
                    .Where(m => m.Status == "Approved")
                    .ToListAsync();

                // Check if matches list is empty
                if (matches.Count == 0)
                {
                    return null;
                }

                return matches;
            }
        }

        public async Task<List<Match>>? GetDeclinedMatchesByUserId(int? userId)
        {
            using (var context = new UsiMentorshipApplicationContext())
            {
                var matches = await context.UserMatches
                    .Where(um => um.UserId == userId)
                    .Join(context.Matches,
                        um => um.MatchId,
                        m => m.MatchId,
                        (um, m) => m)
                    .Where(m => m.Status == "Declined")
                    .ToListAsync();

                return matches;
            }
        }

        public async Task<List<User>> GetUsersFromMatchId(int? matchId, int? userId)
        {
            using UsiMentorshipApplicationContext requestContext = new UsiMentorshipApplicationContext();

            // the userId parameter is the paramter we do not want to get since they're logged in 
            var users = await requestContext.UserMatches
                .Where(um => um.MatchId == matchId && um.UserId != userId)
                // combine UserMatches and Users
                .Join(requestContext.Users,
                    // for UserMatches, combine on UserId column
                    um => um.UserId,
                    // for users, combine on UserId column
                    u => u.UserId,
                    // after finding the matching records, keep information from the Users table
                    (um, u) => u)
                .ToListAsync();

            return users;
        }

        // adding a parameter to specify getting approved, pending or declined matches
        // specify "Pending", "Approved", or "Declined" in the  statusString parameter to get the corresponding matches 
        public async Task<List<MatchRequestDisplayCardObject>>? GetMatchRequestDisplayCards(int? loggedInUserId, string statusString)
        {
            // this method takes in a list of matches and a UserId for the logged in User and gets the corresponding 
            List<MatchRequestDisplayCardObject> matchRequestDisplayCardObjects = null;
            
            List<Match>? matchIds;

            if (statusString == "Pending") 
            {
                matchIds = await GetPendingMatchesByUserId(loggedInUserId);
            }
            else if (statusString == "Approved")
            {
                matchIds = await GetApprovedMatchesByUserId(loggedInUserId);
                if (matchIds == null)
                {
                    return null;
                }
            }
            // else means the string is "Declined"
            else
            {
                matchIds = await GetDeclinedMatchesByUserId(loggedInUserId);
            }

            matchRequestDisplayCardObjects = new List<MatchRequestDisplayCardObject>();

            // foreach match in matches, get the corresponding user and put it in the object
            foreach (Match match in matchIds)
            {
                // get all the other users in the match except the logged in user
                List<User> users = await GetUsersFromMatchId(match.MatchId, loggedInUserId);

                // get the first user since there should only be one in this case
                User? onlyUser = users.FirstOrDefault();
                MatchRequestDisplayCardObject matchRequestObject = new MatchRequestDisplayCardObject { CorrespondingMatch = match, RequestedUser = onlyUser };

                // add the new object to the list
                matchRequestDisplayCardObjects.Add(matchRequestObject);
            }

            return matchRequestDisplayCardObjects;
        }

    }
}
