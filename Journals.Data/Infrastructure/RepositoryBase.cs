using Journals.Core.Common;
using Journals.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Journals.Data.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : BaseEntity
    {
        private DbContext dataContext;
        private readonly IDbSet<T> dbset;
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected DbContext DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }

        public virtual void Add(T entity)
        {
            dbset.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbset.Remove(obj);
        }

        public virtual T GetById(long id)
        {
            return dbset.Find(id);
        }

        public virtual T GetById(string id)
        {
            return dbset.Find(id);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                using (DataContext)
                {
                    return DataContext.Set<T>().Where(predicate).SingleOrDefault();
                }
            }
            else
            {
                throw new ApplicationException("Predicate value must be passed to Get<T>.");
            }
        }

        public virtual IQueryable<T> GetList(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var coll = DataContext.Set<T>();
                if (predicate != null)
                {
                    return coll.Where(predicate);
                }
                return coll;
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public virtual IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy)
        {
            try
            {
                return GetList(predicate).OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public virtual IQueryable<T> GetList<TKey>(Expression<Func<T, TKey>> orderBy)
        {
            try
            {
                return GetList().OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public virtual IQueryable<T> GetList()
        {
            try
            {
                return DataContext.Set<T>();
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public OperationStatus ExecuteStoreCommand(string cmdText, params object[] parameters)
        {
            var opStatus = new OperationStatus { Status = true };

            try
            {
                opStatus.RecordsAffected = DataContext.Database.ExecuteSqlCommand(cmdText, parameters);
            }
            catch (Exception exp)
            {
                OperationStatus.CreateFromException("Error executing store command: ", exp);
            }
            return opStatus;
        }
    }
}