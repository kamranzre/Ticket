using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PagedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public UserRoleType UserRoleType { get; set; }
        public List<T> Items { get; set; } = new List<T>();
    }

    public class Paggination
    {
        public Paggination()
        {
            
        }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string UserId { get; set; }
    }

}
