using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Tickets
{
    public class AssignTicketUpdate
    {
        public string AssignedExpertId { get; set; }

        public TicketStatus Status { get; set; }

        public DateTime AssignedAt { get; set; }
    }
}
