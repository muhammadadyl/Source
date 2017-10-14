using Journals.Core.Common;
using Journals.Data;
using Journals.Data.Infrastructure;
using Journals.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Journals.Repository
{
    public class JournalRepository : RepositoryBase<Journal>, IJournalRepository
    {
        public JournalRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public List<Journal> GetAllJournals(int userId)
        {
            var journals = GetList(j => j.Id > 0 && j.UserId == userId);
            if (journals != null)
                return journals.ToList();
            return new List<Journal>();
        }

        public Journal GetJournalById(int Id)
        {
            return GetList(p => p.Id == Id).Include("Issues").SingleOrDefault();
        }

    }
}