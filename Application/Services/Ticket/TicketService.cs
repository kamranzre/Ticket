using Application.DTO;
using Application.DTO.Tickets;
using Application.Helpers;
using Core.Entities;
using Core.Enums;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Ticket
{
    public class TicketService : BaseService, ITicketService
    {
        private readonly IUserService _userService;

        public TicketService(IUnitOfWork unitOfWork, IUserService userService) : base(unitOfWork)
        {
            _userService = userService;
        }

        public async Task<OperationResult<string>> InsertTicket(InsertTicket model, string userId)
        {
            try
            {
                var TicketCode = CodeGenerator.GenerateTicketCode();
                await UnitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var ticket = new Core.Entities.Ticket
                    {
                        Title = model.Title,
                        TicketCode = TicketCode,
                        UserId = userId,
                        Priority = model.TicketPriority,
                        Status = TicketStatus.Open,
                        CreatedAt = DateTime.Now
                    };

                    ticket.Messages.Add(new Core.Entities.TicketMessage
                    {
                        TicketId = ticket.Id,
                        SenderId = userId,
                        Message = model.Message,
                        CreatedAt = DateTime.Now,
                        IsRead = false,
                    });
                    await UnitOfWork.Tickets.AddAsync(ticket);
                    await UnitOfWork.CompleteAsync();
                });

                return OperationResult<string>.Success(TicketCode, "تیکت با موفقیت ثبت شد");
            }
            catch (Exception)
            {
                return OperationResult<string>.Fail("خطا در ثبت تیکت");
            }
        }

        public async Task<PagedResult<TicketList>> GetAllTickets(Paggination model)
        {
            var userInRole = await _userService.GetUserRoleAsync(model.UserId);
            var query = UnitOfWork.Tickets.AsNoTracking()
                .Include(x => x.Messages)
                .AsQueryable();
            if (userInRole == UserRoleType.Support)
            {
                query = query.Where(x => x.AssignedExpertId == model.UserId || x.AssignedExpertId == null);
            }
            else
            {
                query = query.Where(x => x.UserId == model.UserId);
            }

            var tickets = query.OrderByDescending(x => x.Status)
                  .ThenByDescending(x => x.Priority)
                  .ThenByDescending(x => x.CreatedAt)
                  .Include(a => a.AssignedExpert)
                  .Select(x => new TicketList
                  {
                      Id = x.Id,
                      Title = x.Title,
                      AssignedExpert = x.AssignedExpertId,
                      ClosedAt = x.ClosedAt,
                      CreatedAt = x.CreatedAt,
                      TicketPriority = x.Priority,
                      TicketStatus = x.Status,
                      TicketCode = x.TicketCode,
                      AssignedExpertName = x.AssignedExpert.UserName
                  });
            var data = await GetPagedAsync(tickets, model.PageNumber, model.PageSize);
            data.UserRoleType = (UserRoleType)userInRole;
            return data;
        }

        public async Task<bool> DeleteTicket(int ticketId)
        {
            try
            {
                var item = await UnitOfWork.Tickets.GetByIdAsync(ticketId);
                if (item != null && item.Status == TicketStatus.Open)
                {
                    await UnitOfWork.Tickets.DeleteAsync(ticketId);
                    await UnitOfWork.CompleteAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsExistTicketAsync(string userId, bool isExpert = false)
        {
            if (!isExpert)
            {

                return await UnitOfWork.Tickets.AsNoTracking()
                     .AnyAsync(x => x.UserId == userId && x.Status != TicketStatus.Closed);
            }
            else
            {
                return await UnitOfWork.Tickets.AsNoTracking()
                     .AnyAsync(x => x.AssignedExpertId == userId && x.Status != TicketStatus.Closed);
            }
        }

        public async Task<TicketDetailViewModel> GetTicketDetails(int ticketId)
        {
            var userId = _userService.GetUserId();
            var userInRole = await _userService.GetUserRoleAsync(userId);
            return UnitOfWork.Tickets.AsNoTracking().Where(x => x.Id == ticketId)
                 .Include(x => x.User)
                 .Include(x => x.Messages)
                 .ThenInclude(x => x.Sender)
                 .Select(x => new TicketDetailViewModel
                 {
                     Id = x.Id,
                     TicketCode = x.TicketCode,
                     Title = x.Title,
                     CreatedAt = x.CreatedAt,
                     Status = x.Status,
                     Priority = x.Priority,
                     AssignedExpertId = x.AssignedExpertId,
                     CurrentUserId = userId,
                     UserRole = userInRole.Value,
                     Messages = x.Messages.Select(m => new TicketMessageViewModel
                     {
                         Id = m.Id,
                         TicketId = m.TicketId,
                         SenderId = m.SenderId,
                         SenderName = m.Sender.UserName,
                         Message = m.Message,
                         IsRead = m.IsRead,
                         CreatedAt = m.CreatedAt
                     }).ToList(),
                 }).FirstOrDefault();

        }


        public async Task<bool> AssignToExpoert(int ticketId, string userId)
        {
            try
            {
                //چک شود اساین نشده باشد
                //کارشناس باشد
                //وضعیت به InProsess تغییر کند
                //تاریخ assignAt باید ست شود
                var item = await UnitOfWork.Tickets.GetByIdAsync(ticketId);
                var userRole = await _userService.GetUserRoleAsync(userId);
                if (item.AssignedExpertId is null && userRole == UserRoleType.Support)
                {
                    await UnitOfWork.Tickets.UpdatePartialAsync(ticketId, new AssignTicketUpdate
                    {
                        AssignedAt = DateTime.Now,
                        Status = TicketStatus.InProgress,
                        AssignedExpertId = userId
                    }, item);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<OperationResult<string>> CloseTicket(int ticketId, string userId)
        {
            var ticket = await UnitOfWork.Tickets
                .GetByIdWithIncludeAsync(ticketId, x => x.Messages);

            if (ticket == null)
                return OperationResult<string>.Fail("تیکت یافت نشد");

            var role = await _userService.GetUserRoleAsync(userId);

            if (!CanCloseTicket(ticket, userId, role))
                return OperationResult<string>.Fail("اجازه بستن تیکت را ندارید");

            var vm = new CloseTicketViewModel
            {
                ClosedAt = DateTime.Now,
                Status = TicketStatus.Closed,
                CloseById = userId,
            };

            await UnitOfWork.Tickets.UpdatePartialAsync(ticketId, vm, ticket);
            return OperationResult<string>.Success("بستن تیکت با موفقیت انجام شد");
        }



        public async Task<OperationResult<string>> SendMessage(int ticketId, string message)
        {
            var ticket = await UnitOfWork.Tickets.GetByIdAsync(ticketId);
            if (ticket == null)
                return OperationResult<string>.Fail("تیکت یافت نشد");

            if (ticket.Status == TicketStatus.Closed)
                return OperationResult<string>.Fail("امکان ارسال پیام وجود ندارد");

            var userId = _userService.GetUserId();
            var userRole = await _userService.GetUserRoleAsync(userId);

            bool canSend =
                userRole == UserRoleType.User ||
                (userRole == UserRoleType.Support && ticket.AssignedExpertId == userId);

            if (!canSend)
                return OperationResult<string>.Fail("شما اجازه ارسال پیام ندارید");

            await UnitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var ticketMessage = new Core.Entities.TicketMessage
                {
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    Message = message,
                    SenderId = userId,
                    TicketId = ticketId
                };
                ticket.Status = TicketStatus.Answered;
                await UnitOfWork.Tickets.UpdateAsync(ticket);
                await UnitOfWork.TicketMessages.AddAsync(ticketMessage);
                await UnitOfWork.CompleteAsync();

            });
            return OperationResult<string>.Success(string.Empty, "پیام ارسال شد");

        }

        #region PrivateMethods
        private async Task<PagedResult<T>> GetPagedAsync<T>(IQueryable<T> query, int pageNumber, int pageSize)
        {
            var result = new PagedResult<T>();

            result.PageNumber = pageNumber;
            result.PageSize = pageSize;

            result.TotalCount = await query.CountAsync();

            result.Items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return result;
        }

        private bool CanCloseTicket(Core.Entities.Ticket ticket, string userId, UserRoleType? role)
        {
            if (role == UserRoleType.User)
                return ticket.UserId == userId;

            if (role == UserRoleType.Support)
            {
                var lastMessageSender = ticket.Messages?.OrderByDescending(x => x.CreatedAt).Select(x => x.SenderId).FirstOrDefault();
                var isAssignedToUser = ticket.AssignedExpertId == userId;
                return lastMessageSender == userId && isAssignedToUser;
            }

            return false;
        }

        #endregion
    }
}
