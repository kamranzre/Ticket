using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Ticket:BaseEntity<int>
    {
        public string Title { get; set; }

        public string UserId { get; set; }

        public string? AssignedExpertId { get; set; }

        public TicketStatus Status { get; set; }

        public TicketPriority Priority { get; set; }

        public bool IsActive { get; set; } = true;

        public string? DeletedByUserReason { get; set; }

        public string? DeletedByUserId { get; set; }

        public DateTime? DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? AssignedAt { get; set; }

        public DateTime? ClosedAt { get; set; }


        public ApplicationUser User { get; set; }

        public ApplicationUser? AssignedExpert { get; set; }

        public ICollection<TicketMessage> Messages { get; set; }
    }
}
