using Core.Entities;
using Core.Enums;
using Core.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService: IUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;
        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public string GetUserId()
        {
            return httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<UserRoleType?> GetUserRoleAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return null;

            var roles = await userManager.GetRolesAsync(user);

            var role = roles.FirstOrDefault();

            if (Enum.TryParse<UserRoleType>(role, true, out var result))
                return result;

            return null;
        }

    }
}
