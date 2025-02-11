﻿using BibleExplanationControllers.Models.Bible;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.User
{
    public class SubAdmin : IdentityUser
    {
        public bool CanChangeBooksData { get; set; }

        [ForeignKey("Admin")]
        public string AdminId { get; set; } = string.Empty;

        [JsonIgnore]
        public Admin? Admin { get; set; } = default!;

        [JsonIgnore]
        public ICollection<Worker> Workers { get; set; } = [];

        [JsonIgnore]
        public ICollection<Explanation> Explanations { get; set; } = [];
    }
}
