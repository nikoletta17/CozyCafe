﻿using System;
using System.ComponentModel.DataAnnotations;
using CozyCafe.Models.Domain.Admin;

namespace CozyCafe.Models.Domain.ForUser
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public required string UserId { get; set; }
        
        public ApplicationUser? User { get; set; }

       
        public int MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }
    }
}

