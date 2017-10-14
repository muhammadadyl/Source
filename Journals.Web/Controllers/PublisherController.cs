using AutoMapper;
using Journals.Model;
using Journals.Service.Interfaces;
using Journals.Web.Filters;
using Journals.Web.Helpers;
using Journals.Web.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace Journals.Web.Controllers
{
    [AuthorizeRedirect(Roles = "Publisher")]
    public class PublisherController : Controller
    {
        private IJournalService _journalService;
        private IStaticMembershipService _membershipService;
        private IMapper _mapper;

        public PublisherController(IJournalService journalService, IStaticMembershipService membershipService, IMapper mapper)
        {
            _journalService = journalService;
            _membershipService = membershipService;
            _mapper = mapper;// Mapper Updates cause syntax change
        }

        public ActionResult Index()
        {
            var userId = (int)_membershipService.GetUser().ProviderUserKey;

            var allJournals = _journalService.GetAllJournals(userId);
            var journals = _mapper.Map<List<Journal>, List<JournalViewModel>>(allJournals);// Mapper Updates cause syntax change
            return View(journals);
        }

        public ActionResult Details(int id)
        {
            var selectedJournal = _journalService.GetJournalById(id);
            var journal = _mapper.Map<Journal, JournalViewModel>(selectedJournal);// Mapper Updates cause syntax change
            return View(journal);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JournalViewModel journal)
        {
            if (ModelState.IsValid)
            {
                var newJournal = _mapper.Map<JournalViewModel, Journal>(journal);// Mapper Updates cause syntax change

                newJournal.UserId = (int)_membershipService.GetUser().ProviderUserKey;

                var opStatus = _journalService.AddJournal(newJournal);
                if (!opStatus.Status)
                    throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

                return RedirectToAction("Index");
            }
            else
                return View(journal);
        }

        public ActionResult Delete(int Id)
        {
            var selectedJournal = _journalService.GetJournalById(Id);
            if (selectedJournal.UserId == (int) _membershipService.GetUser().ProviderUserKey)
            {
                var journal = _mapper.Map<Journal, JournalViewModel>(selectedJournal);// Mapper Updates cause syntax change
                return View(journal); 
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(JournalViewModel journal)
        {
            var selectedJournal = _mapper.Map<JournalViewModel, Journal>(journal); // Mapper Updates cause syntax change

            if (selectedJournal?.UserId == (int)_membershipService.GetUser().ProviderUserKey)
            {
                var opStatus = _journalService.DeleteJournal(selectedJournal);
                if (!opStatus.Status)
                    throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            var journal = _journalService.GetJournalById(Id);

            if (journal?.UserId == (int)_membershipService.GetUser().ProviderUserKey)
            {
                var selectedJournal = _mapper.Map<Journal, JournalUpdateViewModel>(journal);// Mapper Updates cause syntax change

                return View(selectedJournal); 
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JournalUpdateViewModel journal)
        {
            if (ModelState.IsValid && journal?.UserId == (int)_membershipService.GetUser().ProviderUserKey)
            {
                var selectedJournal = _mapper.Map<JournalUpdateViewModel, Journal>(journal);// Mapper Updates cause syntax change
                var opStatus = _journalService.UpdateJournal(selectedJournal);
                if (!opStatus.Status)
                    throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

                return RedirectToAction("Index");
            }
            else
                return View(journal);
        }
    }
}