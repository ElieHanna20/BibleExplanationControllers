using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BibleExplanationControllers.Models.User;

namespace BibleExplanationControllers.Data
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>(options)
    {
        public required DbSet<Admin> Admins { get; set; }
        public required DbSet<SubAdmin> SubAdmins { get; set; }
        public required DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Admin - SubAdmin relationship
            modelBuilder.Entity<Admin>()
                .HasMany(a => a.SubAdmins)
                .WithOne(sa => sa.Admin)
                .HasForeignKey(sa => sa.AdminId);

            // SubAdmin - Worker relationship
            modelBuilder.Entity<SubAdmin>()
                .HasMany(sa => sa.Workers)
                .WithOne(w => w.SubAdmin)
                .HasForeignKey(w => w.SubAdminId);

            // SubAdmin - Explanation relationship
            modelBuilder.Entity<SubAdmin>()
                .HasMany(sa => sa.Explanations)
                .WithOne(e => e.SubAdmin)
                .HasForeignKey(e => e.SubAdminId);

            // Worker - Explanation relationship
            modelBuilder.Entity<Worker>()
                .HasMany(w => w.Explanations)
                .WithOne(e => e.Worker)
                .HasForeignKey(e => e.WorkerId);
        }
    }

}
