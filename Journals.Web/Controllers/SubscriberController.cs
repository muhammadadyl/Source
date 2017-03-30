using AutoMapper;
using Journals.Model;
using Journals.Repository;
using Journals.Service.Interfaces;
using Journals.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Security;

namespace Journals.Web.Controllers
{
    [Authorize]
    public class SubscriberController : Controller
    {
        private IJournalService _journalService;
        private ISubscriptionService _subscriptionService;
        private IMapper _mapper;

        public SubscriberController(IJournalService journalService, ISubscriptionService subscriptionService, IMapper mapper)
        {
            _journalService = journalService;
            _subscriptionService = subscriptionService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var journals = _subscriptionService.GetAllJournals();

            if (journals == null)
                return View();

            var userId = (int)Membership.GetUser().ProviderUserKey;
            var subscriptions = _subscriptionService.GetJournalsForSubscriber(userId);

            var subscriberModel = _mapper.Map<List<Journal>, List<SubscriptionViewModel>>(journals);
            foreach (var journal in subscriberModel)
            {
                if (subscriptions.Any(k => k.JournalId == journal.Id))
                    journal.IsSubscribed = true;
            }

            return View(subscriberModel);
        }

        public ActionResult Subscribe(int Id)
        {
            var opStatus = _subscriptionService.AddSubscription(Id, (int)Membership.GetUser().ProviderUserKey);
            if (!opStatus.Status)
                throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            return RedirectToAction("Index");
        }

        public ActionResult UnSubscribe(int Id)
        {
            var opStatus = _subscriptionService.UnSubscribe(Id, (int)Membership.GetUser().ProviderUserKey);
            if (!opStatus.Status)
                throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            return RedirectToAction("Index");
        }
    }
}