using USIMentorshipWebApp.Models;

namespace USIMentorshipWebApp.Data
{
    public class SessionService
    {
        public User SessionUser { get; private set; }
        public List<Role> Roles { get; private set; }

        public List<Role> GetUserRoles(User user)
        {
            using (var context = new UsiMentorshipApplicationContext())
            {
                var roles = (from ur in context.UserRoles
                             join r in context.Roles on ur.RoleId equals r.RoleId
                             where ur.UserId == user.UserId
                             select r).ToList();

                return roles;
            }
        }


        public void Login(User user)
        {
            SessionUser = user;
            Roles = GetUserRoles(user);
        }

        public void Logout()
        {
            SessionUser = null;
            Roles = null;
        }
    }
}
