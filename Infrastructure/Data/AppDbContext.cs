using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                    .HasOne(t => t.User)
                    .WithMany()
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                    .HasOne(t => t.AssignedExpert)
                    .WithMany()
                    .HasForeignKey(t => t.AssignedExpertId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketMessage>()
                    .HasOne(m => m.Ticket)
                    .WithMany(t => t.Messages)
                    .HasForeignKey(m => m.TicketId);

            modelBuilder.Entity<TicketMessage>()
                    .HasOne(m => m.Sender)
                    .WithMany()
                    .HasForeignKey(m => m.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SystemNotification>()
                    .HasOne(n => n.User)
                    .WithMany()
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                    .HasQueryFilter(t => t.IsActive);



            base.OnModelCreating(modelBuilder);
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketMessage> TicketMessages { get; set; }

        public DbSet<SystemNotification> SystemNotifications { get; set; }

    }
}
