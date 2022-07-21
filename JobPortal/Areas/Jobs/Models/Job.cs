using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Areas.Jobs.Models
{
    public class Job
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string JobQualification { get; set; }

        [Required]
        public string JobType { get; set; }

        [Required]
        public string SalaryType { get; set; }

        [Required]
        public string SalaryRange { get; set; }

        [Required]
        public string JobExperience { get; set; }

        [Required]
        public string JobShift { get; set; }

        [Required]
        public string JobLevel { get; set; }

        [Required]
        public bool IsPublished { get; set; }

        [Required]
        public DateTime ExpireDate { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public string JobDescription { get; set; }

        public string JobSpecification { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
