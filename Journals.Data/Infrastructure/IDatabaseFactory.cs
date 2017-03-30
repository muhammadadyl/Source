using System;
using System.Data.Entity;

namespace Journals.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        DbContext Get();
    }
}
