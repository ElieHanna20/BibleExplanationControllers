using Microsoft.EntityFrameworkCore;
using BibleExplanationControllers.Models.User;
using BibleExplanationControllers.Models.Bible;

namespace BibleExplanationControllers.Data
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<SubAdmin> SubAdmins { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Explanation> Explanations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();

                entity.HasDiscriminator<string>("UserType")
                      .HasValue<AppUser>("AppUser")
                      .HasValue<Admin>("Admin")
                      .HasValue<SubAdmin>("SubAdmin")
                      .HasValue<Worker>("Worker");
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasMany(a => a.SubAdmins)
                      .WithOne(sa => sa.Admin)
                      .HasForeignKey(sa => sa.AdminId)
                      .OnDelete(DeleteBehavior.Restrict);
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

            modelBuilder.Entity<Explanation>(entity =>
            {
                entity.HasOne(e => e.Subtitle)
                      .WithMany(s => s.Explanations)
                      .HasForeignKey(e => e.SubtitleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SubAdmin)
                      .WithMany(sa => sa.Explanations)
                      .HasForeignKey(e => e.SubAdminId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Worker)
                      .WithMany(w => w.Explanations)
                      .HasForeignKey(e => e.WorkerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
