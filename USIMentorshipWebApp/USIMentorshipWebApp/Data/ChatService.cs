using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using USIMentorshipWebApp.Models;
using USIMentorshipWebApp.Pages.Chat;

namespace USIMentorshipWebApp.Data
{
    public class ChatService
    {
        private readonly UsiMentorshipApplicationContext _chatContext;
        public ChatService()
        {
            _chatContext = new UsiMentorshipApplicationContext();
        }

        // this works and is good
        public async Task<List<int>> GetMatchIdsWithChats(User user)
        {
            var matchIds = await _chatContext.UserMatches
                .Where(um => um.UserId == user.UserId)
                .Join(_chatContext.MatchCommunicationDetails,
                    um => um.MatchId,
                    mcd => mcd.MatchId,
                    (um, mcd) => new { um, mcd })
                .Where(joined => joined.mcd.CommunicationType == "Chat")
                .Select(joined => joined.um.MatchId)
                .Distinct()
                .ToListAsync();

            return matchIds;
        }

        // method used for UserChatSidebarCard.razor component
        public async Task<List<SidebarUserCard>> GetSidebarUserCardData(User user)
        {
            var userMatchIdsWithChats = await GetMatchIdsWithChats(user);

            List<SidebarUserCard> sidebarCardData = new List<SidebarUserCard>();

            foreach (int matchId in userMatchIdsWithChats)
            {
                var sidebarUsers = await GetUsersFromMatchId(matchId, user.UserId);
                var mostRecentChat = await GetMostRecentChat(matchId);
                SidebarUserCard sidebarUserCard = new SidebarUserCard { MostRecentChat = mostRecentChat, UsersInMatch = sidebarUsers };
                sidebarCardData.Add(sidebarUserCard);
            }

            return sidebarCardData;
        }

        // method used for LoggedInUser.razor component and OtherUserMessage.razor
        public async Task<List<UserMessages>> GetUserMessagesData(int matchId)
        {
            UserService userService = new UserService();

            var chatMessages = await GetChatDetailsForMatch(matchId);

            List<UserMessages> userMessagesList = new List<UserMessages>();

            foreach (MatchCommunicationDetail chat in chatMessages)
            {
                var senderUser = userService.GetUserByIdAsync(chat.SenderUserId);
                var mostRecentChat = await GetMostRecentChat(matchId);
                UserMessages UserMessagesObject = new UserMessages { user = senderUser, userMessage = chat };
                userMessagesList.Add(UserMessagesObject);
            }

            return userMessagesList;
        }

        public async Task<List<User>> GetUsersFromMatchId(int matchId, int userId)
        {
            // the userId parameter is the paramter we do not want to get since they're logged in 
            var users = await _chatContext.UserMatches
                .Where(um => um.MatchId == matchId && um.UserId != userId)
                // combine UserMatches and Users
                .Join(_chatContext.Users,
                    // for UserMatches, combine on UserId column
                    um => um.UserId,
                    // for users, combine on UserId column
                    u => u.UserId,
                    // after finding the matching records, keep information from the Users table
                    (um,  u) => u)
                .ToListAsync();

            return users;
        }

        public async Task<List<int>> GetUserIdsFromMatchId(int matchId, int userId)
        {
            // the userId parameter is the paramter we do not want to get since they're logged in 
            var userIds = await _chatContext.UserMatches
                .Where(um => um.MatchId == matchId && um.UserId != userId)
                .Select(um => um.UserId)
                .ToListAsync();

            return userIds;
        }

        public async Task<List<User>> GetUsersInMatchExceptLoggedInUser(USIMentorshipWebApp.Models.Match match, User loggedInUser)
        {
            var usersInMatch = await _chatContext.UserMatches
                 //This is a join operation between the UserMatches table(after applying the Where filter)
                 //and the Users table. It joins on UserId. The result of the join is a collection of
                 //User objects(denoted by (um, u) => u)
                .Where(um => um.MatchId == match.MatchId && um.UserId != loggedInUser.UserId)
                .Join(_chatContext.Users,
                    um => um.UserId,
                    u => u.UserId,
                    (um, u) => u)
                .ToListAsync();

            return usersInMatch;
        }

        //This method first filters the MatchCommunicationDetail for the given match and where CommunicationType is
        //“Chat”, then orders the result by DateOfCommunication in descending order and selects the first record.
        public async Task<MatchCommunicationDetail> GetMostRecentChat(int matchId)
        {
            UserService userService = new UserService();
            var mostRecentChat = await _chatContext.MatchCommunicationDetails
                .Where(mcd => mcd.MatchId == matchId && mcd.CommunicationType == "Chat")
                .OrderByDescending(mcd => mcd.DateOfCommunication)
                .FirstOrDefaultAsync();

            return mostRecentChat;
        }

        public async Task<List<MatchCommunicationDetail>> GetChatDetailsForMatch(int matchId)
        {
            var chatDetails = await _chatContext.MatchCommunicationDetails
                .Where(mcd => mcd.MatchId == matchId && mcd.CommunicationType == "Chat")
                .OrderBy(mcd => mcd.DateOfCommunication)
                .ToListAsync();

            return chatDetails;
        }

        public async Task SendChat(int matchId, string chatContent, int senderId)
        {
            using UsiMentorshipApplicationContext chatContext2 = new UsiMentorshipApplicationContext();


            MatchCommunicationDetail newChat = new MatchCommunicationDetail 
            { 
                MatchId = matchId,
                SenderUserId = senderId,
                CommunicationType = "Chat",
                CommunicationContent = chatContent,
                DateOfCommunication = DateTime.Now
            };

            // add the match and to the Matches table
            chatContext2.Add(newChat);

            // Need to save the changes so the next queries can be ran on match
            chatContext2.SaveChanges();
        }

    }
}
