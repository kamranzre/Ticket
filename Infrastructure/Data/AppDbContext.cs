using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.TicketCode).IsRequired().HasMaxLength(20);
                entity.HasIndex(t => t.TicketCode).IsUnique();
            });

            modelBuilder.Entity<TicketMessage>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.TicketId).IsRequired();
                entity.Property(t => t.SenderId).IsRequired();
                entity.Property(t => t.Message).IsRequired();
                entity.Property(t => t.IsRead).IsRequired();
                entity.Property(t => t.CreatedAt).IsRequired();
            });
            modelBuilder.Entity<Ticket>()
                    .HasOne(t => t.User)
                    .WithMany(t=>t.Users)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(c => c.CloseBy)
                .WithMany(c => c.CloseBys)
                .HasForeignKey(t => t.CloseById);

            modelBuilder.Entity<Ticket>()
                    .HasOne(t => t.AssignedExpert)
                    .WithMany(t=>t.AssignedExperts)
                    .HasForeignKey(t => t.AssignedExpertId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketMessage>()
                    .HasOne(m => m.Ticket)
                    .WithMany(t => t.Messages)
                    .HasForeignKey(m => m.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketMessage>()
                    .HasOne(m => m.Sender)
                    .WithMany(x=>x.Senders)
                    .HasForeignKey(m => m.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SystemNotification>()
                    .HasOne(n => n.User)
                    .WithMany(x=>x.Notifications)
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade);



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
