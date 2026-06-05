using Application.Services;
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

            services.AddScoped<IDbConnection>(db =>
            new SqlConnection(configuration.GetConnectionString("Ticket")));

            services.AddDbContext<AppIdentityDbContext>(options =>
              options.UseSqlServer(
                  configuration.GetConnectionString("Ticket")));


            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        }
    }
}
