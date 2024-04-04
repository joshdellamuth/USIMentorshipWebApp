using Microsoft.EntityFrameworkCore;
using USIMentorshipWebApp.Models;

namespace USIMentorshipWebApp.Data
{
    public class ChatService
    {
        // add user must take in the name of a role also to make the new user. Every new user must have a role. 
        public async Task<List<Match>> GetUserMatchesWithChats(User user)
        {
            using UsiMentorshipApplicationContext chatContext = new UsiMentorshipApplicationContext();

            // filter the UserMatches table to only include rows where the UserId matches the UserId of the user we’re interested in
            // join this filtered list of UserMatches with the MatchCommunicationDetails table
            // This gives us a new table where each row contains information from both UserMatches and MatchCommunicationDetails for the same MatchId

            var matches = await chatContext.UserMatches
                .Where(um => um.UserId == user.UserId)
                .Join(chatContext.MatchCommunicationDetails,
                    um => um.MatchId,
                mcd => mcd.MatchId,
                (um, mcd) => new { UserMatch = um, MatchCommunicationDetail = mcd })
            .Where(um_mcd => um_mcd.MatchCommunicationDetail.CommunicationType == "Chat")
            .Select(um_mcd => um_mcd.UserMatch.Match)
            .Distinct()
            .ToListAsync();

            return matches;
        }
    }
}
