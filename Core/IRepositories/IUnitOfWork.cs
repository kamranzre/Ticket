using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IUnitOfWork:IDisposable
    {
        public IRepository<Ticket,int> Tickets { get; }
        Task<int> CompleteAsync();
        Task ExecuteInTransactionAsync(Func<Task> action);
    }
}
