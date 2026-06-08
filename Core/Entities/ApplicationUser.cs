using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public bool IsActive { get; set; }

        public string? ResetPasswordCode { get; set; }
        public DateTime? ResetPasswordExpire { get; set; }

        public ICollection<Ticket> Users { get; set; } = new List<Ticket>();
        public ICollection<Ticket> CloseBys { get; set; } = new List<Ticket>();
        public ICollection<Ticket> AssignedExperts { get; set; } = new List<Ticket>();
        public ICollection<TicketMessage> Senders { get; set; } = new List<TicketMessage>();
        public ICollection<SystemNotification> Notifications { get; set; } = new List<SystemNotification>();
        
    }
}
