﻿using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Dtos.ExplanationDtos
{
    public class CreateExplanationRequestDto
    {
        [Required]
        public string Content { get; set; } = string.Empty; // Explanation content

        public int? SubtitleId { get; set; } // Foreign Key: Subtitle

        public Guid? SubAdminId { get; set; } // Foreign Key: SubAdmin

        public Guid? WorkerId { get; set; } // Foreign Key: Worker
    }
}
