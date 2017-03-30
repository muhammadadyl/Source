using Journals.Core.Common;
using Journals.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Journals.Data.Infrastructure
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);

        T GetById(long id);

        T GetById(string id);

        T Get(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetList(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy);

        IQueryable<T> GetList<TKey>(Expression<Func<T, TKey>> orderBy);

        IQueryable<T> GetList();

        OperationStatus ExecuteStoreCommand(string cmdText, params object[] parameters);
    }
}

