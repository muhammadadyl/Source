using System;
using System.Collections.Generic;
using Journals.Data.Infrastructure;
using Journals.Model;
using System.Linq;
using Journals.Core.Common;

namespace Journals.Repository
{
    public class IssueRepository : RepositoryBase<Issue>, IIssueRepository
    {
        public IssueRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public List<Issue> GetAllNewlyAddedIssue(DateTime dateTime)
        {
            try
            {

                List<Issue> list = DataContext.Set<Issue>()
                                          .Where(f => f.CreatedDate > dateTime)
                                          .ToList();

                return list;
            }
            catch (Exception e)
            {
                OperationStatus.CreateFromException("Error fetching subscriptions: ", e); ;
            }

            return new List<Issue>();
        }
    }
}
