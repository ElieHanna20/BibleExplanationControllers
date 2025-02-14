using BibleExplanationControllers.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BibleExplanationControllers.Data
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        public required DbSet<Admin> Admins { get; set; }
        public required DbSet<SubAdmin> SubAdmins { get; set; }
        public required DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Admin>()
                .HasMany(a => a.SubAdmins)
                .WithOne(sa => sa.Admin)
                .HasForeignKey(sa => sa.AdminId);

            modelBuilder.Entity<SubAdmin>()
                .HasMany(sa => sa.Workers)
                .WithOne(w => w.SubAdmin)
                .HasForeignKey(w => w.SubAdminId);

            modelBuilder.Entity<Worker>()
             .HasMany(w => w.Explanations)
             .WithOne(e => e.Worker)
             .HasForeignKey(e => e.WorkerId);
        }
    }
}

