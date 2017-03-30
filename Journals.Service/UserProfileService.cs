using System;
using Journals.Model;
using Journals.Service.Interfaces;
using Journals.Data.Infrastructure;
using Journals.Repository;

namespace Journals.Service
{
    public class UserProfileService : IUserProfileService
    {
        private IUserProfileRepository _userProfileRepository;
        private IUnitOfWork _unitOfWork;

        public UserProfileService(IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork)
        {
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork;
        }

        public UserProfile GetUserByName(string userName)
        {
           return _userProfileRepository.Get(a => a.UserName.ToLower() == userName.ToLower());
        }

        public void Save(UserProfile userProfile)
        {
            _userProfileRepository.Add(userProfile);
            _unitOfWork.Commit();
        }
    }
}
