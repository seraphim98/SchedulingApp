using Scheduler.Database;
using Microsoft.EntityFrameworkCore;
namespace Scheduler.Database
{
    public interface IRepository<T>(DatabaseContext context)
    {
        public void MarkAsModified(T entity);
    }
}
