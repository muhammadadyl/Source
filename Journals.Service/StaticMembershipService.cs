using Journals.Service.Interfaces;
using System.Web.Security;
using WebMatrix.WebData;
using WebMatrix.Data;

namespace Journals.Repository
{
    public class StaticMembershipService : IStaticMembershipService
    {
        public MembershipUser GetUser()
        {
            return Membership.GetUser();
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            var roles = (SimpleRoleProvider)Roles.Provider;
            return roles.IsUserInRole(userName, roleName);
        }
    }
}