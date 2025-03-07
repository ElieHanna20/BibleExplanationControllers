﻿using Microsoft.EntityFrameworkCore;
using BibleExplanationControllers.Models.Bible;

namespace BibleExplanationControllers.Data
{
    public class BibleDbContext(DbContextOptions<BibleDbContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Subtitle> Subtitles { get; set; }
        public DbSet<Verse> Verses { get; set; }
        public DbSet<Explanation> Explanations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().ToTable("Books", "bible");
            modelBuilder.Entity<Chapter>().ToTable("Chapters", "bible");
            modelBuilder.Entity<Subtitle>().ToTable("Subtitles", "bible");
            modelBuilder.Entity<Verse>().ToTable("Verses", "bible");
            modelBuilder.Entity<Explanation>().ToTable("Explanations", "bible");

            modelBuilder.Entity<Book>()
                .HasIndex(b => b.Name)
                .IsUnique();

            modelBuilder.Entity<Chapter>()
                .HasIndex(c => new { c.BookId, c.ChapterNumber })
                .IsUnique();

            modelBuilder.Entity<Subtitle>()
                .HasIndex(s => new { s.ChapterId, s.SubtitleName })
                .IsUnique();

            modelBuilder.Entity<Chapter>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Chapters)
                .HasForeignKey(c => c.BookId);

            modelBuilder.Entity<Subtitle>()
                .HasOne(s => s.Chapter)
                .WithMany(c => c.Subtitles)
                .HasForeignKey(s => s.ChapterId);

            modelBuilder.Entity<Verse>()
                .HasOne(v => v.Subtitle)
                .WithMany(s => s.Verses)
                .HasForeignKey(v => v.SubtitleId);

            modelBuilder.Entity<Explanation>()
                .HasOne(e => e.Subtitle)
                .WithMany(s => s.Explanations)
                .HasForeignKey(e => e.SubtitleId);

            modelBuilder.Entity<Explanation>()
                .HasOne(e => e.SubAdmin)
                .WithMany(sa => sa.Explanations)
                .HasForeignKey(e => e.SubAdminId);

            modelBuilder.Entity<Explanation>()
                .HasOne(e => e.Worker)
                .WithMany(w => w.Explanations)
                .HasForeignKey(e => e.WorkerId);
        }
    }
}
