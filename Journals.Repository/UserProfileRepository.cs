using Journals.Data.Infrastructure;
using Journals.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journals.Repository
{
    public class UserProfileRepository : RepositoryBase<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
