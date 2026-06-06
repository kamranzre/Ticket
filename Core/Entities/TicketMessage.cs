using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TicketMessage:BaseEntity<int>
    {
        public int TicketId { get; set; }

        public string SenderId { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

        public Ticket Ticket { get; set; }

        public ApplicationUser Sender { get; set; }
    }


}
