using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum TicketPriority : byte
    {
        [Display(Name = "کم")] 
        Low = 0,
        [Display(Name = "متوسط")] 
        Normal = 1,
        [Display(Name = "بالا")] 
        High = 2,
    }
}
