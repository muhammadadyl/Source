using Journals.Core.Common;
using Journals.Model;
using System.Collections.Generic;
using System;

namespace Journals.Service.Interfaces
{
    public interface ISubscriptionService
    {
        List<Journal> GetAllJournals();
        List<Subscription> GetJournalsForSubscriber(int userId);
        List<Subscription> GetJournalsForSubscriber(string userName);
        OperationStatus AddSubscription(int journalId, int userId);
        OperationStatus UnSubscribe(int journalId, int userId);
        List<UserProfile> GetSubscriberForJournal(int journalId);
    }
}
