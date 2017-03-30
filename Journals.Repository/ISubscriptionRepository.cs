using Journals.Data.Infrastructure;
using Journals.Model;
using System.Collections.Generic;

namespace Journals.Repository
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        List<Journal> GetAllJournals();
        List<Subscription> GetJournalsForSubscriber(int userId);
        List<Subscription> GetJournalsForSubscriber(string userName);
    }
}