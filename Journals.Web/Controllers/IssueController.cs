using AutoMapper;
using Journals.Model;
using Journals.Service.Interfaces;
using Journals.Web.Filters;
using Journals.Web.Helpers;
using Journals.Web.ViewModels;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace Journals.Web.Controllers
{
    [AuthorizeRedirect(Roles = "Publisher")]
    public class IssueController : Controller
    {
        private IIssueService _issueService;
        private IJournalService _journalService;
        private IStaticMembershipService _membershipService;
        private IMapper _mapper;

        public IssueController(IIssueService issueService, IJournalService journalService, IStaticMembershipService membershipService, IMapper mapper)
        {
            _issueService = issueService;
            _journalService = journalService;
            _membershipService = membershipService;
            _mapper = mapper;// Mapper Updates cause syntax change
        }

        // GET: Issue/Create
        public ActionResult Create(int id)
        {
            return View(new IssueViewModel { JournalId = id });
        }

        // POST: Issue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, IssueViewModel issue)
        {

            var selectedJournal = _journalService.GetJournalById(id);

            if (selectedJournal?.UserId == (int)_membershipService.GetUser().ProviderUserKey)
            {
                issue.JournalId = selectedJournal.Id;
                issue.Version = selectedJournal.Issues.Count() + 1;
                var newIssue = _mapper.Map<IssueViewModel, Issue>(issue);// Mapper Updates cause syntax change
                Helper.PopulateFile(issue.File, newIssue);

                var opStatus = _issueService.AddIssue(newIssue);
                if (!opStatus.Status)
                    throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

                return RedirectToAction("Details", "Publisher", new { Id = newIssue.JournalId });
            }
            return View();
        }

        public ActionResult GetFile(int Id)
        {
            var j = _issueService.GetIssueById(Id);
            if (j == null)
                throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            return File(j.Content, j.ContentType);
        }

        // GET: Issue/Edit/5
        public ActionResult Edit(int id)
        {
            var selectedIssue = _issueService.GetIssueById(id);
            if (selectedIssue?.JournalId > 0)
            {
                if (selectedIssue?.Journal?.UserId == (int)_membershipService.GetUser().ProviderUserKey)
                {
                    var newIssue = _mapper.Map<Issue, IssueViewModel>(selectedIssue);
                    return View(newIssue);
                }
            }
            return RedirectToAction("Details", "Publisher", new { Id = selectedIssue?.JournalId });
        }

        // POST: Issue/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IssueViewModel issue)
        {
            var selectedIssue = _issueService.GetIssueById(id);

            if (selectedIssue?.Journal?.UserId == (int)_membershipService.GetUser().ProviderUserKey)
            {
                Helper.PopulateFile(issue.File, selectedIssue);

                var opStatus = _issueService.UpdateIssue(selectedIssue);
                if (!opStatus.Status)
                    throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

                return RedirectToAction("Details", "Publisher", new { Id = selectedIssue.JournalId });
            }

            return View(issue);
        }

        // GET: Issue/Delete/5
        public ActionResult Delete(int id)
        {
            var selectedIssue = _issueService.GetIssueById(id);
            if (selectedIssue?.JournalId > 0)
            {
                if (selectedIssue?.Journal?.UserId == (int)_membershipService.GetUser().ProviderUserKey)
                {
                    var newIssue = _mapper.Map<Issue, IssueViewModel>(selectedIssue);
                    return View(newIssue);
                }
            }
            return RedirectToAction("Details", "Publisher", new { Id = selectedIssue?.JournalId });
        }

        // POST: Issue/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, IssueViewModel issue)
        {
            var selectedIssue = _issueService.GetIssueById(id);
            if (selectedIssue?.JournalId > 0)
            {
                var selectedJournal = _journalService.GetJournalById(selectedIssue.JournalId);
                if (selectedJournal?.UserId == (int)_membershipService.GetUser().ProviderUserKey)
                {
                    issue.JournalId = selectedJournal.Id;
                    issue.Version = selectedIssue.Version;
                    var newIssue = _mapper.Map<IssueViewModel, Issue>(issue);// Mapper Updates cause syntax change

                    var opStatus = _issueService.DeleteIssue(newIssue);

                    if (!opStatus.Status)
                        throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

                    return RedirectToAction("Details", "Publisher", new { Id = newIssue.JournalId });
                }
            }
            return View(issue);
        }
    }
}
