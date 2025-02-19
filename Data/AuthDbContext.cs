using Microsoft.EntityFrameworkCore;
using BibleExplanationControllers.Models.User;

namespace BibleExplanationControllers.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<SubAdmin> SubAdmins { get; set; }
        public DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();

                entity.HasDiscriminator<string>("UserType")
                      .HasValue<User>("AppUser")
                      .HasValue<Admin>("Admin")
                      .HasValue<SubAdmin>("SubAdmin")
                      .HasValue<Worker>("Worker");
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasIndex(a => a.Username).IsUnique(); // Ensure unique Username
            });

            modelBuilder.Entity<SubAdmin>(entity =>
            {
                entity.HasMany(sa => sa.Workers)
                      .WithOne(w => w.SubAdmin)
                      .HasForeignKey(w => w.SubAdminId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasMany(w => w.Explanations)
                      .WithOne(e => e.Worker)
                      .HasForeignKey(e => e.WorkerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
