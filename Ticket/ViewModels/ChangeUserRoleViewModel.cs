using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketProject.ViewModels
{
    public class ChangeUserRoleViewModel
    {
        public string Message { get; set; }

        public string UserId { get; set; }

        public int Role { get; set; }

        public List<SelectListItem> Users { get; set; }
    }
}
