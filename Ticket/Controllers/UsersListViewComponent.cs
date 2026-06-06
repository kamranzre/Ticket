using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class UsersListViewComponent : ViewComponent
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersListViewComponent(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public IViewComponentResult Invoke()
    {
        var users = _userManager.Users
            .Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            }).ToList();

        return View(users);
    }
}
