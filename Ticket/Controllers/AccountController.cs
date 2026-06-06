using Application.Helpers;
using Core.Constants;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketProject.ViewModels;

namespace TicketProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region SeedCreateAdmin
        [HttpGet]
        public async Task<IActionResult> CreateAdmin()
        {
            var existingUser = await _userManager.FindByNameAsync("admin");

            if (existingUser != null)
                return Content("Admin already exists");

            var user = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@test.com",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, "admin@Test123");

            if (!result.Succeeded)
                return Content("Error creating admin");

            await _userManager.AddToRoleAsync(user, Roles.Admin);

            return Content("Admin created successfully");
        }
        #endregion

        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.User);

                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                false
            );

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid login attempt");

            return View(model);
        }

        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        #endregion

        #region AddRoleToUser

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ChangeUserRole()
        {
            var users = _userManager.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                }).ToList();

            var model = new ChangeUserRoleViewModel
            {
                Users = users
            };

            return View(model);
        }



        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ChangeUserRole(ChangeUserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return View(new ChangeUserRoleViewModel { Message = "کاربر یافت نشد" });

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (model.Role == 1)
                await _userManager.AddToRoleAsync(user, Roles.User);

            if (model.Role == 2)
                await _userManager.AddToRoleAsync(user, Roles.Support);
            return View(new ChangeUserRoleViewModel{ Message = "عملیات با موفقیت انجام شد" });
        }

        #endregion

        #region ForgetPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GenerateResetCode(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return Json(new { success = false, message = "User not found" });

            var code = ResetCodeGenerator.Generate();

            user.ResetPasswordCode = code;
            user.ResetPasswordExpire = DateTime.UtcNow.AddMinutes(10);

            await _userManager.UpdateAsync(user);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyResetCode(string username, string code)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return Json(new { success = false });

            if (user.ResetPasswordCode != code)
                return Json(new { success = false });

            if (user.ResetPasswordExpire < DateTime.UtcNow)
                return Json(new { success = false });

            return Json(new { success = true });
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(string username, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return Json(new { success = false });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
                return Json(new { success = false });

            user.ResetPasswordCode = null;
            user.ResetPasswordExpire = null;

            await _userManager.UpdateAsync(user);

            return Json(new { success = true });
        }



      
        #endregion

    }
}
