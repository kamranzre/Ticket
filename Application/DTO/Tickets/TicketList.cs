using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Tickets
{
    public class TicketList:BaseEntityModel<int>
    {
        public TicketList()
        {
            
        }
        public TicketStatus  TicketStatus { get; set; }

        public TicketPriority TicketPriority { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public string Title { get; set; }

        public string AssignedExpert { get; set; }

        public string AssignedExpertName { get; set; }

        public string TicketCode { get; set; } 

    }
}
