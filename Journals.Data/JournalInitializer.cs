using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using WebMatrix.WebData;

namespace Journals.Data
{
    public class JournalInitializer : DropCreateDatabaseIfModelChanges<JournalsContext>
    {
        protected override void Seed(JournalsContext context)
        {

            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("JournalsDB", "UserProfile", "UserId", "UserName", autoCreateTables: true);

            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists("Publisher"))
            {
                roles.CreateRole("Publisher");
            }
            if (!roles.RoleExists("Subscriber"))
            {
                roles.CreateRole("Subscriber");
            }

            if (!WebSecurity.UserExists("pappu"))
            {
                WebSecurity.CreateUserAndAccount("pappu", "Passw0rd", new { EmailAddress = "smadeelibrahim@yahoo.com" });
            }
            if (!roles.GetRolesForUser("pappu").Contains("Publisher"))
            {
                roles.AddUsersToRoles(new[] { "pappu" }, new[] { "Publisher" });
            }

            if (!WebSecurity.UserExists("pappy"))
            {
                WebSecurity.CreateUserAndAccount("pappy", "Passw0rd", new { EmailAddress = "smadeelibrahim@yahoo.com" });
            }
            if (!roles.GetRolesForUser("pappy").Contains("Subscriber"))
            {
                roles.AddUsersToRoles(new[] { "pappy" }, new[] { "Subscriber" });
            }

            if (!WebSecurity.UserExists("daniel"))
            {
                WebSecurity.CreateUserAndAccount("daniel", "Passw0rd", new { EmailAddress = "smadeelibrahim@yahoo.com" });
            }
            if (!roles.GetRolesForUser("daniel").Contains("Publisher"))
            {
                roles.AddUsersToRoles(new[] { "daniel" }, new[] { "Publisher" });
            }

            if (!WebSecurity.UserExists("andrew"))
            {
                WebSecurity.CreateUserAndAccount("andrew", "Passw0rd", new { EmailAddress = "smadeelibrahim@yahoo.com" });
            }
            if (!roles.GetRolesForUser("andrew").Contains("Subscriber"))
            {
                roles.AddUsersToRoles(new[] { "andrew" }, new[] { "Subscriber" });
            }

            if (!WebSecurity.UserExists("serge"))
            {
                WebSecurity.CreateUserAndAccount("serge", "Passw0rd", new { EmailAddress = "smadeelibrahim@yahoo.com" });
            }
            if (!roles.GetRolesForUser("serge").Contains("Subscriber"))
            {
                roles.AddUsersToRoles(new[] { "serge" }, new[] { "Subscriber" });
            }

            if (!WebSecurity.UserExists("harold"))
            {
                WebSecurity.CreateUserAndAccount("harold", "Passw0rd", new { EmailAddress = "smadeelibrahim@yahoo.com" });
            }
            if (!roles.GetRolesForUser("harold").Contains("Publisher"))
            {
                roles.AddUsersToRoles(new[] { "harold" }, new[] { "Publisher" });
            }
        }
    }
}
