using Journals.Core.Common;
using Journals.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journals.Service.Interfaces
{
    public interface IJournalService
    {
        List<Journal> GetAllJournals(int userId);

        Journal GetJournalById(int Id);

        OperationStatus AddJournal(Journal newJournal);

        OperationStatus DeleteJournal(Journal journal);

        OperationStatus UpdateJournal(Journal journal);
    }
}
