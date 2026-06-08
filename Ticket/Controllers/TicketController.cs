using Application.DTO;
using Application.DTO.Tickets;
using Application.Services;
using Application.Services.Ticket;
using Azure;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TicketProject.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly IUserService userService;
        private readonly ITicketService ticketService;
        public TicketController(IUserService userService, ITicketService ticketService)
        {
            this.userService = userService;
            this.ticketService = ticketService;
        }
        public async Task<IActionResult> Index(Paggination paggination)
        {
                var userId = userService.GetUserId();
            paggination.UserId = userId;
            var response = await ticketService.GetAllTickets(paggination);

            var model = new TicketViewModel
            {
                InsertTicket = new InsertTicket(),
                TicketLists = response.Items,
                PageNumber = response.PageNumber,
                PageSize = response.PageSize,
                TotalCount = response.TotalCount,
                UserRoleType = response.UserRoleType,
                UserId = userId,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(InsertTicket model)
        {
            if (User.IsInRole(UserRoleType.User.ToString()))
            {
                var response = await ticketService.InsertTicket(model, userService.GetUserId());

                return Json(new { data = response });
            }
            else
            {
                return Json(new
                {
                    data = new OperationResult<int>
                    {
                        IsSuccess = false,
                        Message = "ثبت تیکت فقط برای کاربران امکان پذیر است"
                    }
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int ticketid)
        {
            //اگر کاربر یا کارشناس وارد شود بر اساس تاریخ جاری پیام های قبل را خوانده شده میکنیم به عبارتی
            //Isread = true
            var detail = await ticketService.GetTicketDetails(ticketid);
            return View(detail);
        }

        [HttpPost]
        public async Task<IActionResult>SendMessage(int ticketId,string message)
        {
            //باید چک شود که کاربر قصد پیام گذاشتن را دارد یا کارشناس
            //اگر کاربر است فقط پیغام را برای همان تیکت ثبت میکنیم
            //اگر کارشناس است باید چک شود که تیکت به همان کارشناس اساین شده یا نه در صورت عدم اساین اجازه ارسال پیام را نمیدهیم
            //برای ثبت پیغام کاربر هم باید آیدی  کاربر با آیدی تیکت بررسی شود که آیا تیکت مربوط به خود کاربر است یا خیر
            var send = await ticketService.SendMessage(ticketId, message);
            return Json(send);
        }

        [HttpPost]
        public async Task<IActionResult>Delete(int ticketId)
        {
            if (User.IsInRole(UserRoleType.User.ToString()))
            {
            var delete =await ticketService.DeleteTicket(ticketId);
            return Json(delete);
            }
            return Json(false);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToExpert(int ticketId)
        {
            var res = await ticketService.AssignToExpoert(ticketId, userService.GetUserId());
            return Json(res);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseTicket(int ticketId)
        {
            var res = await ticketService.CloseTicket(ticketId, userService.GetUserId());
            return Json(res);
        }
    }
}
