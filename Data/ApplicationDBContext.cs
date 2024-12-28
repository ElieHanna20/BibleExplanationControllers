﻿using BibleExplanationControllers.Models.Bible;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Data
{
    public class ApplicationDBContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public required DbSet<Book> Books { get; set; }
        public required DbSet<Chapter> Chapters { get; set; }
        public required DbSet<Subtitle> Subtitles { get; set; }
        public required DbSet<Verse> Verses { get; set; }
        public required DbSet<Explanation> Explanations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the schema
            modelBuilder.Entity<Book>().ToTable("Books", "bible");
            modelBuilder.Entity<Chapter>().ToTable("Chapters", "bible");
            modelBuilder.Entity<Subtitle>().ToTable("Subtitles", "bible");
            modelBuilder.Entity<Verse>().ToTable("Verses", "bible");
            modelBuilder.Entity<Explanation>().ToTable("Explanations", "bible");

            // Enforce uniqueness on 'Name' field of the Book model
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.Name)
                .IsUnique();

            // Enforce a composite unique index on BookId and ChapterNumber
            modelBuilder.Entity<Chapter>()
                .HasIndex(c => new { c.BookId, c.ChapterNumber })
                .IsUnique();

            // Enforce a composite unique index on ChapterId and SubtitleName
            modelBuilder.Entity<Subtitle>()
                .HasIndex(s => new { s.ChapterId, s.SubtitleName })
                .IsUnique();

            // Relationships
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
        }

    }
}