using Application.DTO;
using Application.DTO.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Ticket
{
    public interface ITicketService
    {
        Task<OperationResult<string>> InsertTicket(InsertTicket model,string userId);
        Task<PagedResult<TicketList>> GetAllTickets(Paggination model);
        Task<bool>DeleteTicket(int ticketId);
        Task<bool> IsExistTicketAsync(string userId, bool isExpert = false);
        Task<TicketDetailViewModel> GetTicketDetails(int ticketId);
        Task<bool> AssignToExpoert(int ticketId, string userId);
        Task<OperationResult<string>> CloseTicket(int ticketId, string userId);
        Task<OperationResult<string>> SendMessage(int ticketId, string message);
    }
}
