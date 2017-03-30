using Journals.Data.Infrastructure;
using Journals.Model;

namespace Journals.Repository
{
    public class IssueRepository : RepositoryBase<Issue>, IIssueRepository
    {
        public IssueRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
