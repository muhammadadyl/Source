using Journals.Core.Common;
using Journals.Data.Infrastructure;
using Journals.Model;
using System.Collections.Generic;

namespace Journals.Repository
{
    public interface IJournalRepository : IRepository<Journal>
    {
        List<Journal> GetAllJournals(int userId);

        Journal GetJournalById(int Id);
    }
}