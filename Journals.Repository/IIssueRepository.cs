using System;
using System.Collections.Generic;
using Journals.Data.Infrastructure;
using Journals.Model;

namespace Journals.Repository
{
    public interface IIssueRepository : IRepository<Issue>
    {
        List<Issue> GetAllNewlyAddedIssue(DateTime dateTime);
    }
}
