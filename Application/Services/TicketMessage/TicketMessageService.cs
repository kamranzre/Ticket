using Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.TicketMessage
{
    public class TicketMessageService:BaseService, ITicketMessageService
    {
        public TicketMessageService(IUnitOfWork unitOfWork):base(unitOfWork)
        {
            
        }
    }
}
