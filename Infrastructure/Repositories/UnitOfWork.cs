using Core.Entities;
using Core.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : DbContextBase, IUnitOfWork
    {
        public UnitOfWork(AppDbContext context, string connectionString) : base(context)
        {
            Tickets = new Repository<Ticket, int>(_context, connectionString);
            TicketMessages = new Repository<TicketMessage, int>(_context, connectionString);
        }

        public IRepository<Ticket, int> Tickets { get; set; }
        public IRepository<TicketMessage, int> TicketMessages { get; set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await action();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }
        }

        protected override void ConfigureEntities(ModelBuilder modelBuilder)
        {
        }
    }
}
