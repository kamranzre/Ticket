using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO.Tickets
{
    public class InsertTicket
    {
        public InsertTicket()
        {
            
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "وارد کردن عنوان الزامی است.")] 
        public string Title { get; set; }

        public TicketPriority TicketPriority { get; set; }

        [Required(ErrorMessage = "نوشتن توضیحات در خصوص تیکت اجباری است.")] 
        public string Message { get; set; }

        //[JsonIgnore]
        //public string UserId { get; set; }

        public string ExpertId { get; set; }
    }
}
