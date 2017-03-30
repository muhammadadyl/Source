using System.Web.Security;

namespace Journals.Service.Interfaces
{
    public interface IStaticMembershipService
    {
        MembershipUser GetUser();

        bool IsUserInRole(string userName, string roleName);
    }
}