using BaigiamasisDarbas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaigiamasisDarbas.Database
{
    public class MeetingDbContext:DbContext
    {
        public DbSet<Worker> Workers { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<Meeting > Meetings { get; set; } = null!;
        public DbSet<MeetingParticipant> Participants { get; set; } = null!;

        public MeetingDbContext() 
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeetingParticipant>()
                .HasKey(mp => new { mp.MeetingId, mp.ParticipantId });

            modelBuilder.Entity<MeetingParticipant>()
                .HasOne(mp => mp.Meeting)
                .WithMany(m => m.Participants)
                .HasForeignKey(mp => mp.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MeetingParticipant>()
                .HasOne(mp => mp.Participant)
                .WithMany()
                .HasForeignKey(mp => mp.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.ResponsiblePerson)
                .WithMany()
                .HasForeignKey(m => m.ResponsiblePersonId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-8V5PSN2;Database=MeetingDatabase;Integrated Security=True;Trusted_Connection=true;TrustServerCertificate=true;");
        }



    }
}
