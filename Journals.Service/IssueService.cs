using System;
using Journals.Core.Common;
using Journals.Data.Infrastructure;
using Journals.Model;
using Journals.Repository;
using Journals.Service.Interfaces;

namespace Journals.Service
{
    public class IssueService : IIssueService
    {
        private IIssueRepository _issueRepository;
        private IUnitOfWork _unitOfWork;
        public IssueService(IIssueRepository journalRepository, IUnitOfWork unitOfWork)
        {
            _issueRepository = journalRepository;
            _unitOfWork = unitOfWork;
        }

        public OperationStatus AddIssue(Issue issue)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                _issueRepository.Add(issue);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                opStatus = OperationStatus.CreateFromException("Error adding Issue: ", e);
            }

            return opStatus;
        }

        public OperationStatus DeleteIssue(Issue issue)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                var tobeDeletedIssue = _issueRepository.GetById(issue.Id); 
                _issueRepository.Delete(tobeDeletedIssue);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                opStatus = OperationStatus.CreateFromException("Error deleting Issue: ", e);
            }

            return opStatus;
        }

        public Issue GetIssueById(int id)
        {
           return _issueRepository.GetById(id);
        }

        public OperationStatus UpdateIssue(Issue issue)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                var i = _issueRepository.GetById(issue.Id);

                if (issue.Content != null)
                    i.Content = issue.Content;

                if (issue.ContentType != null)
                    i.ContentType = issue.ContentType;

                if (issue.FileName != null)
                    i.FileName = issue.FileName;

                i.ModifiedDate = DateTime.Now;

                _issueRepository.Update(issue);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                opStatus = OperationStatus.CreateFromException("Error updating Issue: ", e);
            }

            return opStatus;
        }
    }
}
