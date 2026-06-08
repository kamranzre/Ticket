using Core.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Tickets
{
    public class TicketDetailViewModel:BaseEntity<int>
    {
        public string Title { get; set; }
        public string TicketCode { get; set; }
        public string UserName { get; set; }
        public string AssignedExpertId { get; set; }
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public UserRoleType UserRole { get; set; }

        public string CurrentUserId { get; set; }

        public List<TicketMessageViewModel> Messages { get; set; } = new List<TicketMessageViewModel>();

        public string NewMessageText { get; set; }
    }

    public class TicketMessageViewModel : BaseEntity<int>
    {
        public int TicketId { get; set; }
        public string SenderId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SenderName { get; set; }
    }
}
