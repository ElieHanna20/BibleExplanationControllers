﻿namespace BibleExplanationControllers.Dtos.WorkerDtos
{
    public class WorkerUpdateDto
    {
        public string? Username { get; set; } 
        public string? Password { get; set; } 
        public bool? CanChangeBooksData { get; set; }
    }
}
