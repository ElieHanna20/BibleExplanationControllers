using System;
using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Models.User
{
    public class AppUser
    {
        [Key]
        public Guid Id { get; set; } // Primary key generated as a GUID

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string PasswordHash { get; set; } // Store hashed passwords

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
