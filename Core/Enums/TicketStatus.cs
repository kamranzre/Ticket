using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum TicketStatus : byte
    {
        Open = 0,
        InProgress = 1,
        Answered = 2,
        Closed = 3
    }
}
