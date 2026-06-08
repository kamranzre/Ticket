using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public abstract class DbContextBase : IDisposable
    {
        protected AppDbContext _context;
        protected DbContextBase(AppDbContext context)
        {
            _context = context;
        }

        protected abstract void ConfigureEntities(ModelBuilder modelBuilder);

        public void Dispose()
        {
        }
    }
}
