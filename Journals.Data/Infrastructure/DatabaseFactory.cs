using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journals.Data.Infrastructure
{
    public class DatabaseFactory<T> : Disposable, IDatabaseFactory where T : DbContext, new()
    {
        private DbContext dataContext;
        public DbContext Get()
        {
            return dataContext ?? (dataContext = new T());
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
