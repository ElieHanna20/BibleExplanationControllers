using BibleExplanationControllers.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.Bible
{
    public class Explanation
    {
        [Key]
        public int Id { get; set; } // Primary Key for EF Core internal use

        [Required]
        public string Content { get; set; } = string.Empty; // Explanation content

        [ForeignKey("Subtitle")]
        public int? SubtitleId { get; set; } // Foreign Key: Links to Subtitle (nullable)

        [JsonIgnore]
        public Subtitle? Subtitle { get; set; } // Navigation property for the related subtitle (nullable)

        [ForeignKey("SubAdmin")]
        public int? SubAdminId { get; set; } // Foreign Key for SubAdmin

        [JsonIgnore]
        public SubAdmin? SubAdmin { get; set; } // Navigation property for managing SubAdmin

        [ForeignKey("Worker")]
        public int? WorkerId { get; set; } // Foreign Key for Worker

        [JsonIgnore]
        public Worker? Worker { get; set; } // Navigation property for managing Worker
    }
}
