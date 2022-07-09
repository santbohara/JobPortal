﻿using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class JobCategory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
    }
}