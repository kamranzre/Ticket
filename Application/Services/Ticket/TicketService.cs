using Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Ticket
{
    public class TicketService:BaseService, ITicketService
    {
        public TicketService(IUnitOfWork unitOfWork) :base(unitOfWork)
        {
            
        }
    }
}
