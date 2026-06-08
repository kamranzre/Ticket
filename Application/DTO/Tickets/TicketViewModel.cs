using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Tickets
{
    public class TicketViewModel
    {
        public TicketViewModel()
        {
        }

        public InsertTicket InsertTicket { get; set; }
        public List<TicketList> TicketLists { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public UserRoleType UserRoleType { get; set; }
        public string UserId { get; set; }
    }
}
