using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Ticket : BaseEntity<int>
    {
        public Ticket() { }

        public TicketStatus TicketStatus { get; set; }

        public TicketPriority TicketPriority { get; set; }
    }
}
