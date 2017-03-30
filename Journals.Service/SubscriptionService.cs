using Journals.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Journals.Core.Common;
using Journals.Model;
using Journals.Repository;
using Journals.Data.Infrastructure;

namespace Journals.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private ISubscriptionRepository _subscriptionRepository;
        private IUnitOfWork _unitOfWork;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public List<Journal> GetAllJournals()
        {
            return _subscriptionRepository.GetAllJournals();
        }

        public List<Subscription> GetJournalsForSubscriber(string userName)
        {
            return _subscriptionRepository.GetJournalsForSubscriber(userName);
        }

        public List<Subscription> GetJournalsForSubscriber(int userId)
        {
            return _subscriptionRepository.GetJournalsForSubscriber(userId);
        }

        public OperationStatus AddSubscription(int journalId, int userId)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                Subscription s = new Subscription();
                s.JournalId = journalId;
                s.UserId = userId;
                _subscriptionRepository.Add(s);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                opStatus = OperationStatus.CreateFromException("Error adding subscription: ", e);
            }

            return opStatus;
        }

        public OperationStatus UnSubscribe(int journalId, int userId)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                var subscriptions = _subscriptionRepository.GetList(u => u.JournalId == journalId && u.UserId == userId);

                foreach (var s in subscriptions)
                {
                    _subscriptionRepository.Delete(s);
                    _unitOfWork.Commit();
                }

            }
            catch (Exception e)
            {
                opStatus = OperationStatus.CreateFromException("Error deleting subscription: ", e);
            }

            return opStatus;
        }
    }
}
