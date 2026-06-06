using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum TicketStatus : byte
    {
        [Display(Name = "باز")]
        Open = 0,
        [Display(Name = "در حال بررسی")]
        InProgress = 1,
        [Display(Name = "پاسخ داده شده")] 
        Answered = 2,
        [Display(Name = "بسته شده")] 
        Closed = 3
    }
}
