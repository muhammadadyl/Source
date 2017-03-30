using Journals.Model;

namespace Journals.Service.Interfaces
{
    public interface IUserProfileService 
    {
        UserProfile GetUserByName(string userName);
        void Save(UserProfile userProfile);
    }
}
