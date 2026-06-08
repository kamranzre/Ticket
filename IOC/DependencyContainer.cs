using Application.Services;
using Application.Services.Ticket;
using Application.Services.TicketMessage;
using Core.Entities;
using Core.IRepositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;

namespace IOC
{
    public static class DependencyContainer
    {
        public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("Ticket")));

            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWork>(sp =>
            {
                var context = sp.GetRequiredService<AppDbContext>();
                var connStr = configuration.GetConnectionString("Ticket");
                return new UnitOfWork(context, connStr);
            });
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ITicketMessageService, TicketMessageService>();

        }
    }
}
