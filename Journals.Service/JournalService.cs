using Journals.Service.Interfaces;
using System;
using System.Collections.Generic;
using Journals.Core.Common;
using Journals.Model;
using Journals.Repository;
using Journals.Data.Infrastructure;

namespace Journals.Service
{
    public class JournalService : IJournalService
    {
        private IJournalRepository _journalRepository;
        private IUnitOfWork _unitOfWork;
        public JournalService(IJournalRepository journalRepository, IUnitOfWork unitOfWork)
        {
            _journalRepository = journalRepository;
            _unitOfWork = unitOfWork;
        }

        public List<Journal> GetAllJournals(int userId)
        {
            return _journalRepository.GetAllJournals(userId);
        }

        public Journal GetJournalById(int Id)
        {
            return _journalRepository.GetJournalById(Id);
        }

        public OperationStatus AddJournal(Journal newJournal)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                newJournal.ModifiedDate = DateTime.Now;
                _journalRepository.Add(newJournal);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                opStatus = OperationStatus.CreateFromException("Error adding journal: ", e);
            }

            return opStatus;
        }

        public OperationStatus DeleteJournal(Journal journal)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                var journalToBeDeleted = _journalRepository.GetById(journal.Id);
                _journalRepository.Delete(journalToBeDeleted);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                opStatus = OperationStatus.CreateFromException("Error deleting journal: ", e);
            }

            return opStatus;
        }

        public OperationStatus UpdateJournal(Journal journal)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                var j = _journalRepository.GetById(journal.Id);

                if (journal.Title != null)
                    j.Title = journal.Title;

                if (journal.Description != null)
                    j.Description = journal.Description;

                if (journal.Content != null)
                    j.Content = journal.Content;

                if (journal.ContentType != null)
                    j.ContentType = journal.ContentType;

                if (journal.FileName != null)
                    j.FileName = journal.FileName;

                j.ModifiedDate = DateTime.Now;

                _journalRepository.Update(j);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                opStatus = OperationStatus.CreateFromException("Error updating journal: ", e);
            }

            return opStatus;
        }
    }
}
