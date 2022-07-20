using JobPortal.Areas.Config.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Areas.Jobs.ViewModel
{
    public class JobViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? ExpireDate { get; set; }

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

        public string? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }

        [Required]
        [Display(Name = "Job Specification")]
        public string JobSpecification { get; set; }

        public bool IsPublished { get; set; }

        public List<SelectListItem>? JobCategories { get; set; }

        public List<SelectListItem>? JobQualifications { get; set; }

        public List<SelectListItem>? JobTypes { get; set; }

        public List<SelectListItem>? SalaryTypes { get; set; }

        public List<SelectListItem>? SalaryRanges { get; set; }

        public List<SelectListItem>? JobExperiences { get; set; }

        public List<SelectListItem>? JobShifts { get; set; }

        public List<SelectListItem>? JobLevels { get; set; }
    }
}
