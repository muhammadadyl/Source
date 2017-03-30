using Journals.Data.Infrastructure;
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
    }
}
