using Microsoft.EntityFrameworkCore;
using USIMentorshipWebApp.Models;

namespace USIMentorshipWebApp.Data
{
    public class ChatService
    {
        // add user must take in the name of a role also to make the new user. Every new user must have a role. 
        public async Task<List<Match>> GetUserMatches(User user)
        {
            using UsiMentorshipApplicationContext chatContext = new UsiMentorshipApplicationContext();

            UserService userService = new UserService();

            // filter the UserMatches table to only include rows where the UserId matches the UserId of the user we’re interested in
            // join this filtered list of UserMatches with the MatchCommunicationDetails table
            // This gives us a new table where each row contains information from both UserMatches and MatchCommunicationDetails for the same MatchId
            var matches = await chatContext.UserMatches
                .Where(um => um.UserId == user.UserId)
                // the above part of the query got all the matches that the passed in user was involved in 
                .Join(chatContext.MatchCommunicationDetails,
                    um => um.MatchId,
                    mcd => mcd.MatchId,
                    (um, mcd) => new { UserMatch = um, MatchCommunicationDetail = mcd })
                // it then joins UserMatches and MatchCommunicationDetails on the UserId field
            .Where(um_mcd => um_mcd.MatchCommunicationDetail.CommunicationType == "Chat")
            // the above only gets MatchCommunicationDetails where the CommunicationType is "Chat"
            .Select(um_mcd => um_mcd.UserMatch.Match)
            .Distinct()
            .ToListAsync();

            return matches;
        }
    }
}
