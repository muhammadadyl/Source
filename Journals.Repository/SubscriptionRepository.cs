﻿using Journals.Core.Common;
using Journals.Data.Infrastructure;
using Journals.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Journals.Repository
{
    public class SubscriptionRepository : RepositoryBase<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }

        public List<Journal> GetAllJournals()
        {
            try
            {
                var result = from a in DataContext.Set<Journal>()
                             where a.Title != null
                             select new { a.Id, a.Title, a.Description, a.User, a.UserId, a.ModifiedDate, a.FileName };

                if (result == null)
                    return new List<Journal>();

                List<Journal> list = result.AsEnumerable()
                                          .Select(f => new Journal
                                          {
                                              Id = f.Id,
                                              Title = f.Title,
                                              Description = f.Description,
                                              UserId = f.UserId,
                                              User = f.User,
                                              ModifiedDate = f.ModifiedDate,
                                              FileName = f.FileName
                                          }).ToList();

                return list;
            }
            catch (Exception e)
            {
                OperationStatus.CreateFromException("Error fetching subscriptions: ", e); ;
            }

            return new List<Journal>();
        }

        public List<Subscription> GetJournalsForSubscriber(int userId)
        {
            try
            {
                    var subscriptions = GetList(u => u.UserId == userId);
                    if (subscriptions != null)
                        return subscriptions.ToList();
            }
            catch (Exception e)
            {
                OperationStatus.CreateFromException("Error fetching subscriptions: ", e); ;
            }

            return new List<Subscription>();
        }

        public List<Subscription> GetJournalsForSubscriber(string userName)
        {
            try
            {

                    var subscriptions = DataContext.Set<Subscription>().Include("Journal").Where(u => u.User.UserName == userName);
                    if (subscriptions != null)
                        return subscriptions.ToList();
            }
            catch (Exception e)
            {
                OperationStatus.CreateFromException("Error fetching subscriptions: ", e); ;
            }

            return new List<Subscription>();
        }
    }
}